using Carola.BusinessLayer.Abstract;
using Carola.DtoLayer.LocationDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LocationController : Controller
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _locationService.TGetListAsync();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateLocationDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _locationService.TCreateLocationAsync(dto);
            TempData["Success"] = "Lokasyon başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var location = await _locationService.TGetByIdDtoAsync(id);
            if (location == null) return NotFound();

            var dto = new UpdateLocationDto
            {
                LocationId = location.LocationId,
                LocationName = location.LocationName,
                AuthorizedPerson = location.AuthorizedPerson,
                City = location.City,
                Address = location.Address,
                ImageUrl = location.ImageUrl
            };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateLocationDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _locationService.TUpdateLocationAsync(dto);
            TempData["Success"] = "Lokasyon başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _locationService.TDeleteAsync(id);
            TempData["Success"] = "Lokasyon silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}