using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Carola.BusinessLayer.Abstract;
using Carola.DtoLayer.CustomerDtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Carola.BusinessLayer.Concrete
{
    public class GeminiOcrService : IOcrService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GeminiOcrService> _logger;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        // Sırayla denenecek modeller — biri yoğunsa diğerine düşer
        private readonly string[] _fallbackModels = new[]
        {
            "gemini-2.5-flash",        // İlk tercih (en stabil + hızlı)
            "gemini-2.5-flash-lite",   // Daha az yoğun, hızlı
            "gemini-2.0-flash"         // Yedek
        };

        public GeminiOcrService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<GeminiOcrService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            _apiKey = configuration["GeminiSettings:ApiKey"]
                      ?? throw new InvalidOperationException("Gemini ApiKey appsettings.json'da tanımlı değil!");
            _baseUrl = configuration["GeminiSettings:BaseUrl"] ?? "https://generativelanguage.googleapis.com/v1beta";
        }

        public async Task<LicenseOcrResultDto> ExtractLicenseDataAsync(byte[] imageBytes, string mimeType)
        {
            try
            {
                var base64Image = Convert.ToBase64String(imageBytes);

                var prompt = @"
Bu bir Türkiye Cumhuriyeti sürücü belgesi (ehliyet) görselidir.
Görseldeki bilgileri çıkartıp SADECE aşağıdaki JSON formatında döndür.
Hiçbir açıklama veya markdown ekleme. Sadece geçerli JSON.

ŞEMA:
{
  ""firstName"": ""string (ehliyet üzerindeki ad, büyük harf)"",
  ""lastName"": ""string (ehliyet üzerindeki soyad, büyük harf)"",
  ""birthDate"": ""string (DD.MM.YYYY formatında doğum tarihi)"",
  ""driverLicenseNumber"": ""string (ehliyet seri numarası)"",
  ""driverLicenseClass"": ""string (ehliyet sınıfı, örn: B, B1, C, D)"",
  ""driverLicenseIssueDate"": ""string (DD.MM.YYYY formatında veriliş tarihi)"",
  ""success"": true,
  ""errorMessage"": null
}

Eğer görsel ehliyet değilse veya bilgiler okunamıyorsa:
{
  ""success"": false,
  ""errorMessage"": ""Görsel bir Türk ehliyeti olarak tanınamadı""
}

Tarihleri DD.MM.YYYY formatında ver. Adı ve soyadı tüm büyük harfle ver.";

                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new object[]
                            {
                                new { text = prompt },
                                new
                                {
                                    inline_data = new
                                    {
                                        mime_type = mimeType,
                                        data = base64Image
                                    }
                                }
                            }
                        }
                    },
                    generationConfig = new
                    {
                        temperature = 0.1,
                        responseMimeType = "application/json"
                    }
                };

                var json = JsonSerializer.Serialize(requestBody);

                // ============== RETRY + FALLBACK MEKANİZMASI ==============
                string? responseBody = null;
                bool success = false;

                foreach (var model in _fallbackModels)
                {
                    _logger.LogInformation("Gemini OCR — model deneniyor: {Model}", model);

                    // Her model için 3 deneme yap (503 / 429 için exponential backoff)
                    for (int attempt = 1; attempt <= 3; attempt++)
                    {
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        var url = $"{_baseUrl}/models/{model}:generateContent?key={_apiKey}";

                        try
                        {
                            var response = await _httpClient.PostAsync(url, content);
                            responseBody = await response.Content.ReadAsStringAsync();

                            if (response.IsSuccessStatusCode)
                            {
                                _logger.LogInformation("Gemini OCR başarılı — model: {Model}, deneme: {Attempt}", model, attempt);
                                success = true;
                                break;
                            }

                            // 503 (overloaded) veya 429 (rate limit) — retry edilebilir
                            if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable ||
                                response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                            {
                                int waitMs = attempt * 1500; // 1.5s, 3s, 4.5s
                                _logger.LogWarning("Model {Model} yoğun ({StatusCode}). {WaitMs}ms bekleniyor (deneme {Attempt}/3)",
                                    model, response.StatusCode, waitMs, attempt);
                                await Task.Delay(waitMs);
                                continue;
                            }

                            // 404, 403 vs. — bu modelle çözülmez, bir sonraki modele geç
                            _logger.LogError("Gemini API kritik hatası ({StatusCode}): {Body}", response.StatusCode, responseBody);
                            break; // bir sonraki modele geç
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Gemini API isteği sırasında istisna");
                        }
                    }

                    if (success) break; // dış foreach'ten de çık
                }

                if (!success || string.IsNullOrEmpty(responseBody))
                {
                    return new LicenseOcrResultDto
                    {
                        Success = false,
                        ErrorMessage = "Yapay zeka servisi şu anda çok yoğun. Lütfen 1-2 dakika sonra tekrar deneyin."
                    };
                }

                // ============== JSON PARSE ==============
                using var doc = JsonDocument.Parse(responseBody);
                var candidates = doc.RootElement.GetProperty("candidates");
                if (candidates.GetArrayLength() == 0)
                {
                    return new LicenseOcrResultDto
                    {
                        Success = false,
                        ErrorMessage = "OCR servisi bir cevap üretemedi. Lütfen daha net bir görsel deneyin."
                    };
                }

                var ocrJsonText = candidates[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                if (string.IsNullOrWhiteSpace(ocrJsonText))
                {
                    return new LicenseOcrResultDto
                    {
                        Success = false,
                        ErrorMessage = "OCR sonucu boş döndü."
                    };
                }

                _logger.LogInformation("Gemini cevap: {Json}", ocrJsonText);

                var parsed = JsonSerializer.Deserialize<GeminiOcrRawResult>(ocrJsonText, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (parsed == null)
                {
                    return new LicenseOcrResultDto
                    {
                        Success = false,
                        ErrorMessage = "OCR cevabı çözümlenemedi."
                    };
                }

                if (!parsed.Success)
                {
                    return new LicenseOcrResultDto
                    {
                        Success = false,
                        ErrorMessage = parsed.ErrorMessage ?? "Ehliyet bilgileri okunamadı."
                    };
                }

                return new LicenseOcrResultDto
                {
                    Success = true,
                    FirstName = parsed.FirstName ?? "",
                    LastName = parsed.LastName ?? "",
                    BirthDate = TryParseDate(parsed.BirthDate),
                    DriverLicenseNumber = parsed.DriverLicenseNumber ?? "",
                    DriverLicenseClass = parsed.DriverLicenseClass ?? "",
                    DriverLicenseIssueDate = TryParseDate(parsed.DriverLicenseIssueDate)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gemini OCR sırasında beklenmeyen hata");
                return new LicenseOcrResultDto
                {
                    Success = false,
                    ErrorMessage = "OCR servisi sırasında beklenmeyen bir hata oluştu: " + ex.Message
                };
            }
        }

        private static DateTime? TryParseDate(string? dateStr)
        {
            if (string.IsNullOrWhiteSpace(dateStr)) return null;

            string[] formats =
            {
                "dd.MM.yyyy", "d.M.yyyy", "dd/MM/yyyy", "d/M/yyyy",
                "yyyy-MM-dd", "yyyy/MM/dd"
            };

            if (DateTime.TryParseExact(dateStr.Trim(), formats,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out var dt))
            {
                return dt;
            }

            if (DateTime.TryParse(dateStr, out var fallback))
                return fallback;

            return null;
        }

        private class GeminiOcrRawResult
        {
            [JsonPropertyName("firstName")] public string? FirstName { get; set; }
            [JsonPropertyName("lastName")] public string? LastName { get; set; }
            [JsonPropertyName("birthDate")] public string? BirthDate { get; set; }
            [JsonPropertyName("driverLicenseNumber")] public string? DriverLicenseNumber { get; set; }
            [JsonPropertyName("driverLicenseClass")] public string? DriverLicenseClass { get; set; }
            [JsonPropertyName("driverLicenseIssueDate")] public string? DriverLicenseIssueDate { get; set; }
            [JsonPropertyName("success")] public bool Success { get; set; }
            [JsonPropertyName("errorMessage")] public string? ErrorMessage { get; set; }
        }
    }
}