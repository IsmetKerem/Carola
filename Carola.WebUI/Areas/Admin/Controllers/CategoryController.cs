using Carola.BusinessLayer.Abstract;
using Carola.DtoLayer.CategoryDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _categoryService.TGetListAsync();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _categoryService.TCreateCategoryAsync(dto);
            TempData["Success"] = "Kategori başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryService.TGetByIdDtoAsync(id);
            if (category == null) return NotFound();

            var dto = new UpdateCategoryDto
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                CategoryImage = category.CategoryImage
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCategoryDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _categoryService.TUpdateCategoryAsync(dto);
            TempData["Success"] = "Kategori başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.TDeleteAsync(id);
            TempData["Success"] = "Kategori silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}