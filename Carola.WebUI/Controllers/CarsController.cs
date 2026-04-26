using Carola.BusinessLayer.Abstract;
using Carola.DtoLayer.CarDtos;
using Carola.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarService _carService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;
        private readonly ICommentService _commentService;     // YENİ
        private readonly ILocationService _locationService;   // YENİ

        public CarsController(
            ICarService carService,
            IBrandService brandService,
            ICategoryService categoryService,
            ICommentService commentService,
            ILocationService locationService)
        {
            _carService = carService;
            _brandService = brandService;
            _categoryService = categoryService;
            _commentService = commentService;
            _locationService = locationService;
        }

        // ARAÇ LİSTESİ (önceki gibi)
        public async Task<IActionResult> Index(CarFilterDto filter)
        {
            if (filter.PickupDate.HasValue && filter.ReturnDate.HasValue
                && filter.ReturnDate.Value <= filter.PickupDate.Value)
            {
                filter.ReturnDate = filter.PickupDate.Value.AddDays(1);
            }
            if (filter.PageNumber < 1) filter.PageNumber = 1;
            if (filter.PageSize < 1) filter.PageSize = 6;

            var pagedCars = await _carService.TGetFilteredCarsAsync(filter);

            var viewModel = new CarsListViewModel
            {
                CarsPage = pagedCars,
                Filter = filter,
                Brands = await _brandService.TGetListAsync(),
                Categories = await _categoryService.TGetListAsync(),
                MaxAvailablePrice = await _carService.TGetMaxDailyPriceAsync()
            };

            return View(viewModel);
        }

        // ARAÇ DETAY
        public async Task<IActionResult> Detail(int id)
        {
            var car = await _carService.TGetCarByIdWithDetailsAsync(id);
            if (car == null) return NotFound();

            var comments = await _commentService.TGetCommentsByCarIdAsync(id);

            var sameCategoryCars = await _carService.TGetFilteredCarsAsync(new CarFilterDto
            {
                CategoryId = car.CategoryId,
                PageNumber = 1,
                PageSize = 4
            });
            var relatedCars = sameCategoryCars.Items.Where(c => c.CarId != id).Take(3).ToList();

            var locations = await _locationService.TGetListAsync();

            var viewModel = new CarDetailViewModel
            {
                Car = car,
                Comments = comments,
                RelatedCars = relatedCars,
                Locations = locations
            };

            return View(viewModel);
        }
    }
}