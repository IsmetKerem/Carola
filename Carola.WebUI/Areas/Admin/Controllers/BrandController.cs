using Carola.BusinessLayer.Abstract;
using Carola.DtoLayer.BrandDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        
        public async Task<IActionResult> Index()
        {
            var brands = await _brandService.TGetListAsync();
            return View(brands);
        }

        // EKLE
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateBrandDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _brandService.TCreateBrandAsync(dto);
            TempData["Success"] = "Marka başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        // GÜNCELLE
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var brand = await _brandService.TGetByIdDtoAsync(id);
            if (brand == null) return NotFound();

            var updateDto = new UpdateBrandDto
            {
                BrandId = brand.BrandId,
                BrandName = brand.BrandName,
                LogoUrl = brand.LogoUrl,
                Status = brand.Status
            };
            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateBrandDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _brandService.TUpdateBrandAsync(dto);
            TempData["Success"] = "Marka başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        // SIL
        public async Task<IActionResult> Delete(int id)
        {
            await _brandService.TDeleteAsync(id);
            TempData["Success"] = "Marka silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}