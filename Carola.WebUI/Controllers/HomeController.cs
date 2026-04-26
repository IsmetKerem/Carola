using Carola.BusinessLayer.Abstract;
using Carola.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly IBrandService _brandService;
        private readonly IWhyUsService _whyUsService;
        private readonly ICarService _carService;
        private readonly ILocationService _locationService;
        private readonly ICategoryService _categoryService;
        private readonly IContactService _contactService;

        public HomeController(
            ISliderService sliderService,
            IBrandService brandService,
            IWhyUsService whyUsService,
            ICarService carService,
            ILocationService locationService,
            ICategoryService categoryService,
            IContactService contactService)
        {
            _sliderService = sliderService;
            _brandService = brandService;
            _whyUsService = whyUsService;
            _carService = carService;
            _locationService = locationService;
            _categoryService = categoryService;
            _contactService = contactService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel
            {
                Sliders         = await _sliderService.TGetActiveSlidersAsync(),
                Categories      = await _categoryService.TGetListAsync(),
                WhyUsItems      = await _whyUsService.TGetListAsync(),
                FeaturedBrands  = await _brandService.TGetFeaturedBrandsAsync(),
                AllBrands       = await _brandService.TGetListAsync(),
                FeaturedCars    = await _carService.TGetLast6CarsAsync(),
                Locations       = await _locationService.TGetListAsync()
            };
            return View(model);
        }

        public IActionResult About() => View();

        [HttpGet]
        public IActionResult Contact() => View();

        [HttpPost]
        public async Task<IActionResult> Contact(Carola.DtoLayer.ContactDtos.CreateContactDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _contactService.TCreateContactAsync(dto);
            TempData["Success"] = "Mesajınız başarıyla gönderildi. En kısa sürede size dönüş yapacağız.";
            return RedirectToAction(nameof(Contact));
        }
    }
}