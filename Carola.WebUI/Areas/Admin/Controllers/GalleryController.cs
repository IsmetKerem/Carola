using Carola.BusinessLayer.Abstract;
using Carola.DtoLayer.GalleryDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GalleryController : Controller
    {
        private readonly IGalleryService _galleryService;

        public GalleryController(IGalleryService galleryService)
        {
            _galleryService = galleryService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _galleryService.TGetListAsync();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateGalleryDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _galleryService.TCreateGalleryAsync(dto);
            TempData["Success"] = "Görsel galeriye eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var item = await _galleryService.TGetByIdDtoAsync(id);
            if (item == null) return NotFound();

            var dto = new UpdateGalleryDto
            {
                GalleryId = item.GalleryId,
                ImageUrl = item.ImageUrl
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateGalleryDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _galleryService.TUpdateGalleryAsync(dto);
            TempData["Success"] = "Görsel güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _galleryService.TDeleteAsync(id);
            TempData["Success"] = "Görsel silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}