using Carola.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _customerService.TGetListAsync();
            return View(list);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var customer = await _customerService.TGetByIdDtoAsync(id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _customerService.TDeleteAsync(id);
            TempData["Success"] = "Müşteri kaydı silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}