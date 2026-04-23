using Carola.BusinessLayer.Abstract;
using Carola.DtoLayer.CommentDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Carola.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly ICarService _carService;

        public CommentController(ICommentService commentService, ICarService carService)
        {
            _commentService = commentService;
            _carService = carService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _commentService.TGetListAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadCarsDropdown();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCommentDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadCarsDropdown();
                return View(dto);
            }
            await _commentService.TCreateCommentAsync(dto);
            TempData["Success"] = "Yorum başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var item = await _commentService.TGetByIdDtoAsync(id);
            if (item == null) return NotFound();

            var dto = new UpdateCommentDto
            {
                CommentId = item.CommentId,
                UserName = item.UserName,
                UserImage = item.UserImage,
                CommentText = item.CommentText,
                Rating = item.Rating,
                CarId = item.CarId
            };
            await LoadCarsDropdown();
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCommentDto dto)
        {
            if (!ModelState.IsValid)
            {
                await LoadCarsDropdown();
                return View(dto);
            }
            await _commentService.TUpdateCommentAsync(dto);
            TempData["Success"] = "Yorum güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _commentService.TDeleteAsync(id);
            TempData["Success"] = "Yorum silindi.";
            return RedirectToAction(nameof(Index));
        }

        // Tüm araçları dropdown için ViewBag'e koyar
        private async Task LoadCarsDropdown()
        {
            var cars = await _carService.TGetCarsWithBrandAndCategoryAsync();
            ViewBag.Cars = cars.Select(c => new SelectListItem
            {
                Value = c.CarId.ToString(),
                Text = $"{c.BrandName} {c.Model} ({c.PlateNumber})"
            }).ToList();
        }
    }
}