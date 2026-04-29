using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Carola.BusinessLayer.Abstract;
using Carola.DtoLayer.ChatDtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Carola.BusinessLayer.Concrete
{
    public class GeminiChatService : IChatService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GeminiChatService> _logger;
        private readonly ICarService _carService;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        private readonly string[] _fallbackModels = new[]
        {
            "gemini-2.5-flash",
            "gemini-2.5-flash-lite",
            "gemini-2.0-flash"
        };

        public GeminiChatService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<GeminiChatService> logger,
            ICarService carService)
        {
            _httpClient = httpClient;
            _logger = logger;
            _carService = carService;

            _apiKey = configuration["GeminiSettings:ApiKey"]
                      ?? throw new InvalidOperationException("Gemini ApiKey appsettings.json'da tanımlı değil!");
            _baseUrl = configuration["GeminiSettings:BaseUrl"] ?? "https://generativelanguage.googleapis.com/v1beta";
        }

        public async Task<ChatResponseDto> RecommendCarsAsync(string userMessage)
        {
            try
            {
                var allCars = await _carService.TGetCarsWithBrandAndCategoryAsync();
                var availableCars = allCars.Where(c => c.IsAvailable).ToList();

                if (!availableCars.Any())
                {
                    return new ChatResponseDto
                    {
                        Success = false,
                        ErrorMessage = "Sistemde gösterilebilecek araç bulunamadı."
                    };
                }

                var fleetData = availableCars.Select(c => new
                {
                    id = c.CarId,
                    brand = c.BrandName,
                    model = c.Model,
                    category = c.CategoryName,
                    year = c.ModelYear,
                    fuel = c.FuelType,
                    transmission = c.TransmissionType,
                    seats = c.SeatCount,
                    luggage = c.LuggageCapacity,
                    daily_price = c.DailyPrice
                }).ToList();

                var fleetJson = JsonSerializer.Serialize(fleetData);

                // 3. Prompt — sade, net, JSON şemalı
                var prompt = $@"
Sen Carola Araç Kiralama firmasının yapay zeka asistanısın. Adın 'Carola Asistan'.
Kullanıcı bir araç önerisi istiyor. Sana aşağıda mevcut filomuz JSON formatında veriliyor.

GÖREVIN:
1. Kullanıcının mesajını anla (bütçe, kategori, koltuk sayısı, yakıt tipi, vites, kullanım amacı vb.)
2. Filodan en uygun 1-3 aracı seç
3. Her araç için kısa bir gerekçe yaz
4. Genel bir tavsiye paragrafı yaz

ÇIKTI FORMATI — sadece geçerli JSON döndür, hiç açıklama yapma:
{{
  ""replyText"": ""string (1-2 paragraf, sıcak ve profesyonel ton, kullanıcıya hitap)"",
  ""recommendedCarIds"": [
    {{ ""carId"": 1, ""reason"": ""Neden uygun (1 cümle)"" }},
    {{ ""carId"": 5, ""reason"": ""Neden uygun (1 cümle)"" }}
  ]
}}

KURALLAR:
- En fazla 3 araç öner.
- Eğer kullanıcı net istek belirtmediyse, popüler/dengeli araçlar öner ve sorusunu açmasını rica et.
- Eğer mesaj araç kiralama ile alakasızsa: replyText'e nazikçe 'Ben sadece araç önerisi konusunda yardımcı olabiliyorum' yaz, recommendedCarIds boş döndür.
- Türkçe cevap ver. Resmi ama samimi ton.
- Fiyatları '₺' ile yaz.

KULLANICI MESAJI: ""{userMessage.Replace("\"", "'")}""

MEVCUT FİLO:
{fleetJson}
";

                string? responseBody = null;
                bool success = false;

                foreach (var model in _fallbackModels)
                {
                    for (int attempt = 1; attempt <= 2; attempt++)
                    {
                        var requestBody = new
                        {
                            contents = new[]
                            {
                                new
                                {
                                    parts = new object[] { new { text = prompt } }
                                }
                            },
                            generationConfig = new
                            {
                                temperature = 0.4,
                                responseMimeType = "application/json"
                            }
                        };

                        var json = JsonSerializer.Serialize(requestBody);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        var url = $"{_baseUrl}/models/{model}:generateContent?key={_apiKey}";

                        try
                        {
                            var response = await _httpClient.PostAsync(url, content);
                            responseBody = await response.Content.ReadAsStringAsync();

                            if (response.IsSuccessStatusCode)
                            {
                                _logger.LogInformation("Chat OK — model: {Model}", model);
                                success = true;
                                break;
                            }

                            if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable ||
                                response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                            {
                                await Task.Delay(attempt * 1500);
                                continue;
                            }

                            _logger.LogError("Gemini Chat API hatası ({StatusCode}): {Body}", response.StatusCode, responseBody);
                            break;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Gemini Chat istek hatası");
                        }
                    }
                    if (success) break;
                }

                if (!success || string.IsNullOrEmpty(responseBody))
                {
                    return new ChatResponseDto
                    {
                        Success = false,
                        ErrorMessage = "Asistan şu anda yoğun. Lütfen birkaç saniye sonra tekrar deneyin."
                    };
                }

                using var doc = JsonDocument.Parse(responseBody);
                var candidates = doc.RootElement.GetProperty("candidates");
                if (candidates.GetArrayLength() == 0)
                {
                    return new ChatResponseDto { Success = false, ErrorMessage = "Asistan cevap üretemedi." };
                }

                var aiJsonText = candidates[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                if (string.IsNullOrWhiteSpace(aiJsonText))
                {
                    return new ChatResponseDto { Success = false, ErrorMessage = "Boş cevap döndü." };
                }

                var aiResult = JsonSerializer.Deserialize<GeminiChatRawResult>(aiJsonText, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (aiResult == null)
                {
                    return new ChatResponseDto { Success = false, ErrorMessage = "Cevap çözümlenemedi." };
                }

                var recommendedCars = new List<RecommendedCarDto>();
                if (aiResult.RecommendedCarIds != null)
                {
                    foreach (var rec in aiResult.RecommendedCarIds.Take(3))
                    {
                        var car = availableCars.FirstOrDefault(c => c.CarId == rec.CarId);
                        if (car != null)
                        {
                            recommendedCars.Add(new RecommendedCarDto
                            {
                                CarId = car.CarId,
                                BrandName = car.BrandName,
                                Model = car.Model,
                                CategoryName = car.CategoryName,
                                FuelType = car.FuelType,
                                TransmissionType = car.TransmissionType,
                                SeatCount = car.SeatCount,
                                DailyPrice = car.DailyPrice,
                                ImageUrl = car.ImageUrl,
                                Reason = rec.Reason ?? ""
                            });
                        }
                    }
                }

                return new ChatResponseDto
                {
                    Success = true,
                    ReplyText = aiResult.ReplyText ?? "Size yardımcı olabilirim. Ne tür bir araç arıyorsunuz?",
                    RecommendedCars = recommendedCars
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Chat servisi sırasında hata");
                return new ChatResponseDto
                {
                    Success = false,
                    ErrorMessage = "Bir hata oluştu, lütfen tekrar deneyin."
                };
            }
        }

        private class GeminiChatRawResult
        {
            [JsonPropertyName("replyText")] public string? ReplyText { get; set; }
            [JsonPropertyName("recommendedCarIds")] public List<RecommendedCarItem>? RecommendedCarIds { get; set; }
        }

        private class RecommendedCarItem
        {
            [JsonPropertyName("carId")] public int CarId { get; set; }
            [JsonPropertyName("reason")] public string? Reason { get; set; }
        }
    }
}