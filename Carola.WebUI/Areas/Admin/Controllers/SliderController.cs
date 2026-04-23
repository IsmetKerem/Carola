using Carola.BusinessLayer.Abstract;
using Carola.DtoLayer.SliderDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SliderController : Controller
    {
        private readonly ISliderService _sliderService;

        public SliderController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _sliderService.TGetListAsync();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateSliderDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _sliderService.TCreateSliderAsync(dto);
            TempData["Success"] = "Slider başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var item = await _sliderService.TGetByIdDtoAsync(id);
            if (item == null) return NotFound();

            var dto = new UpdateSliderDto
            {
                SliderId = item.SliderId,
                Title = item.Title,
                Description = item.Description,
                BackgroundImage = item.BackgroundImage,
                Image = item.Image,
                Status = item.Status
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateSliderDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _sliderService.TUpdateSliderAsync(dto);
            TempData["Success"] = "Slider başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _sliderService.TDeleteAsync(id);
            TempData["Success"] = "Slider silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}