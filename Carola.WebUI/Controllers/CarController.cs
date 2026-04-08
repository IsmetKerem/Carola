using Carola.BusinessLayer.Abstract;
using Carola.EntityLayer.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Carola.WebUI.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly ICategoryService _categoryService;

        public CarController(ICarService carService, ICategoryService categoryService)
        {
            _carService = carService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> CarList()
        {
            var values = await _carService.TGetAllAsync();
            return View(values);
        }

        public async Task<IActionResult> CreateCar()
        {
            ViewBag.Categories = new SelectList(await _categoryService.TGetAllAsync(), "CategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCar(Car car)
        {
            await _carService.TInsertAsync(car);
            return RedirectToAction("CarList");
        }
    }
}
