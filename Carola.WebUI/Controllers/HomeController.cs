using Carola.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBrandService _brandService;

        public HomeController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public async Task<IActionResult> Index()
        {
            var brands = await _brandService.TGetFeaturedBrandsAsync();
            return View(brands);
        }
    }
}