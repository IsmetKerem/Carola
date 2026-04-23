using Carola.BusinessLayer.Abstract;
using Carola.DtoLayer.VideoDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class VideoController : Controller
    {
        private readonly IVideoService _videoService;

        public VideoController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _videoService.TGetListAsync();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateVideoDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _videoService.TCreateVideoAsync(dto);
            TempData["Success"] = "Video eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var item = await _videoService.TGetByIdDtoAsync(id);
            if (item == null) return NotFound();

            var dto = new UpdateVideoDto
            {
                VideoId = item.VideoId,
                VideoUrl = item.VideoUrl
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateVideoDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _videoService.TUpdateVideoAsync(dto);
            TempData["Success"] = "Video güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _videoService.TDeleteAsync(id);
            TempData["Success"] = "Video silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}