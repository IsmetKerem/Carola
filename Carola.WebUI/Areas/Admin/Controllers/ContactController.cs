using Carola.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _contactService.TGetListAsync();
            // En yeni mesajlar üstte
            list = list.OrderByDescending(c => c.SendDate).ToList();
            return View(list);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var msg = await _contactService.TGetByIdDtoAsync(id);
            if (msg == null) return NotFound();

            
            if (!msg.IsRead)
                await _contactService.TMarkAsReadAsync(id);

            return View(msg);
        }

        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _contactService.TMarkAsReadAsync(id);
            TempData["Success"] = "Mesaj okundu olarak işaretlendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _contactService.TDeleteAsync(id);
            TempData["Success"] = "Mesaj silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}