using Carola.BusinessLayer.Abstract;
using Carola.DtoLayer.CustomerDtos;
using Carola.DtoLayer.ReservationDtos;
using Carola.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Controllers
{
    public class BookingController : Controller
    {
        private readonly ICarService _carService;
        private readonly ILocationService _locationService;
        private readonly ICustomerService _customerService;
        private readonly IReservationService _reservationService;
        private readonly IOcrService _ocrService;
        private readonly IWebHostEnvironment _env;

        public BookingController(
            ICarService carService,
            ILocationService locationService,
            ICustomerService customerService,
            IReservationService reservationService, IOcrService ocrService, IWebHostEnvironment env)
        {
            _carService = carService;
            _locationService = locationService;
            _customerService = customerService;
            _reservationService = reservationService;
            _ocrService = ocrService;
            _env = env;
        }

        public async Task<IActionResult> Index(
            int carId,
            DateTime? pickupDate,
            DateTime? returnDate,
            int pickupLocationId,
            int returnLocationId)
        {
            if (!pickupDate.HasValue) pickupDate = DateTime.Today;
            if (!returnDate.HasValue) returnDate = DateTime.Today.AddDays(3);
            if (returnDate.Value <= pickupDate.Value)
                returnDate = pickupDate.Value.AddDays(1);

            var car = await _carService.TGetCarByIdWithDetailsAsync(carId);
            if (car == null) return NotFound();

            var locations = await _locationService.TGetListAsync();
            var pickup = locations.FirstOrDefault(l => l.LocationId == pickupLocationId)
                         ?? locations.FirstOrDefault();
            var ret = locations.FirstOrDefault(l => l.LocationId == returnLocationId)
                      ?? locations.FirstOrDefault();

            if (pickup == null || ret == null)
            {
                TempData["Error"] = "Lokasyon bilgisi eksik. Lütfen alış ve iade lokasyonu seçin.";
                return RedirectToAction("Detail", "Cars", new { id = carId });
            }

            var viewModel = new BookingViewModel
            {
                CarId = carId,
                PickupDate = pickupDate.Value,
                ReturnDate = returnDate.Value,
                PickupLocationId = pickup.LocationId,
                ReturnLocationId = ret.LocationId,
                Car = car,
                PickupLocation = pickup,
                ReturnLocation = ret
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Complete(CompleteBookingDto dto)
        {
            if (dto.ReturnDate <= dto.PickupDate)
            {
                TempData["Error"] = "İade tarihi alış tarihinden sonra olmalıdır.";
                return RedirectToAction(nameof(Index), new
                {
                    carId = dto.CarId,
                    pickupDate = dto.PickupDate,
                    returnDate = dto.ReturnDate,
                    pickupLocationId = dto.PickupLocationId,
                    returnLocationId = dto.ReturnLocationId
                });
            }

            
            var availableCars = await _carService.TGetAvailableCarsBetweenDatesAsync(dto.PickupDate, dto.ReturnDate);
            if (!availableCars.Any(c => c.CarId == dto.CarId))
            {
                TempData["Error"] = "Üzgünüz, bu araç seçtiğiniz tarihler arasında artık müsait değil.";
                return RedirectToAction("Detail", "Cars", new { id = dto.CarId });
            }

            
            int customerId;
            var existingCustomer = await _customerService.TGetCustomerByEmailAsync(dto.Email);
            if (existingCustomer != null)
            {
                customerId = existingCustomer.CustomerId;
                await _customerService.TUpdateCustomerAsync(new UpdateCustomerDto
                {
                    CustomerId = customerId,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    BirthDate = dto.BirthDate,
                    DriverLicenseNumber = dto.DriverLicenseNumber,
                    DriverLicenseClass = dto.DriverLicenseClass,
                    DriverLicenseIssueDate = dto.DriverLicenseIssueDate,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    LicenseImageUrl = dto.LicenseImageUrl
                });
            }
            else
            {
                customerId = await _customerService.TCreateCustomerAndReturnIdAsync(new CreateCustomerDto
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    BirthDate = dto.BirthDate,
                    DriverLicenseNumber = dto.DriverLicenseNumber,
                    DriverLicenseClass = dto.DriverLicenseClass,
                    DriverLicenseIssueDate = dto.DriverLicenseIssueDate,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    LicenseImageUrl = dto.LicenseImageUrl
                });
            }

            await _reservationService.TCreateReservationAsync(new CreateReservationDto
            {
                CarId = dto.CarId,
                CustomerId = customerId,
                PickupDate = dto.PickupDate,
                ReturnDate = dto.ReturnDate,
                PickupLocationId = dto.PickupLocationId,
                ReturnLocationId = dto.ReturnLocationId,
                TotalPrice = dto.TotalPrice,
                Description = dto.Description ?? ""
            });

            return RedirectToAction(nameof(Success));
        }

        public IActionResult Success()
        {
            return View();
        }

        [HttpPost]
        [RequestSizeLimit(10_000_000)] // 10 MB max
        public async Task<IActionResult> UploadLicense(IFormFile licenseFile)
        {
            if (licenseFile == null || licenseFile.Length == 0)
            {
                return Json(new LicenseOcrResultDto
                {
                    Success = false,
                    ErrorMessage = "Dosya seçilmedi."
                });
            }

            // 5 MB üzeri kabul etme (frontend'de de kontrol var ama burada da)
            if (licenseFile.Length > 5 * 1024 * 1024)
            {
                return Json(new LicenseOcrResultDto
                {
                    Success = false,
                    ErrorMessage = "Dosya boyutu 5 MB'dan büyük olamaz."
                });
            }

            // İzin verilen mime tipleri
            var allowed = new[] { "image/jpeg", "image/jpg", "image/png", "image/webp" };
            if (!allowed.Contains(licenseFile.ContentType.ToLower()))
            {
                return Json(new LicenseOcrResultDto
                {
                    Success = false,
                    ErrorMessage = "Sadece JPG, PNG veya WEBP formatında görsel yükleyebilirsiniz."
                });
            }

            try
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "licenses");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                var fileExt = Path.GetExtension(licenseFile.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}{fileExt}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await licenseFile.CopyToAsync(stream);
                }

                var publicUrl = $"/uploads/licenses/{uniqueFileName}";

                byte[] imageBytes;
                using (var ms = new MemoryStream())
                {
                    using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    await fs.CopyToAsync(ms);
                    imageBytes = ms.ToArray();
                }

                var result = await _ocrService.ExtractLicenseDataAsync(imageBytes, licenseFile.ContentType);
                result.LicenseImageUrl = publicUrl;

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new LicenseOcrResultDto
                {
                    Success = false,
                    ErrorMessage = "Sunucu hatası: " + ex.Message
                });
            }
        }
        
        
    }
}