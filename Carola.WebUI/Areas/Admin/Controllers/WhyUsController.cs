using Carola.BusinessLayer.Abstract;
using Carola.DtoLayer.WhyUsDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class WhyUsController : Controller
    {
        private readonly IWhyUsService _whyUsService;

        public WhyUsController(IWhyUsService whyUsService)
        {
            _whyUsService = whyUsService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _whyUsService.TGetListAsync();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateWhyUsDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _whyUsService.TCreateWhyUsAsync(dto);
            TempData["Success"] = "Madde başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var item = await _whyUsService.TGetByIdDtoAsync(id);
            if (item == null) return NotFound();

            var dto = new UpdateWhyUsDto
            {
                WhyUsId = item.WhyUsId,
                IconUrl = item.IconUrl,
                Title = item.Title,
                Description = item.Description
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateWhyUsDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _whyUsService.TUpdateWhyUsAsync(dto);
            TempData["Success"] = "Madde başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _whyUsService.TDeleteAsync(id);
            TempData["Success"] = "Madde silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}