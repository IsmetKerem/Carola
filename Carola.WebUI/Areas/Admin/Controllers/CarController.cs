using Carola.BusinessLayer.Abstract;
using Carola.DtoLayer.CarDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Carola.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;

        public CarController(ICarService carService, IBrandService brandService, ICategoryService categoryService)
        {
            _carService = carService;
            _brandService = brandService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _carService.TGetCarsWithBrandAndCategoryAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCarDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(dto);
            }
            await _carService.TCreateCarAsync(dto);
            TempData["Success"] = "Araç başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var car = await _carService.TGetCarByIdWithDetailsAsync(id);
            if (car == null) return NotFound();

            var dto = new UpdateCarDto
            {
                CarId = car.CarId,
                Model = car.Model,
                ModelYear = car.ModelYear,
                PlateNumber = car.PlateNumber,
                DailyPrice = car.DailyPrice,
                SeatCount = car.SeatCount,
                LuggageCapacity = car.LuggageCapacity,
                Mileage = car.Mileage,
                IsAvailable = car.IsAvailable,
                FuelType = car.FuelType,
                TransmissionType = car.TransmissionType,
                ImageUrl = car.ImageUrl,
                Description = car.Description,
                BrandId = car.BrandId,
                CategoryId = car.CategoryId
            };
            await LoadDropdowns();
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCarDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(dto);
            }
            await _carService.TUpdateCarAsync(dto);
            TempData["Success"] = "Araç başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _carService.TDeleteAsync(id);
            TempData["Success"] = "Araç silindi.";
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdowns()
        {
            var brands = await _brandService.TGetListAsync();
            var categories = await _categoryService.TGetListAsync();

            ViewBag.Brands = brands.Select(b => new SelectListItem
            {
                Value = b.BrandId.ToString(),
                Text = b.BrandName
            }).ToList();

            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            }).ToList();

            // Statik dropdown'lar
            ViewBag.FuelTypes = new List<SelectListItem>
            {
                new() { Value = "Benzin", Text = "Benzin" },
                new() { Value = "Dizel",  Text = "Dizel" },
                new() { Value = "LPG",    Text = "LPG" },
                new() { Value = "Hybrid", Text = "Hybrid" },
                new() { Value = "Elektrik", Text = "Elektrik" }
            };

            ViewBag.TransmissionTypes = new List<SelectListItem>
            {
                new() { Value = "Manuel",   Text = "Manuel" },
                new() { Value = "Otomatik", Text = "Otomatik" },
                new() { Value = "Yarı Otomatik", Text = "Yarı Otomatik" }
            };
        }
    }
}