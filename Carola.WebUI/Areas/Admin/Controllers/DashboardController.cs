using Carola.BusinessLayer.Abstract;
using Carola.EntityLayer.Enums;
using Carola.WebUI.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ICarService _carService;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;
        private readonly ILocationService _locationService;
        private readonly IReservationService _reservationService;
        private readonly IContactService _contactService;
        private readonly ICustomerService _customerService;

        public DashboardController(
            ICarService carService,
            IBrandService brandService,
            ICategoryService categoryService,
            ILocationService locationService,
            IReservationService reservationService,
            IContactService contactService,
            ICustomerService customerService)
        {
            _carService = carService;
            _brandService = brandService;
            _categoryService = categoryService;
            _locationService = locationService;
            _reservationService = reservationService;
            _contactService = contactService;
            _customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel
            {
                TotalCars         = (await _carService.TGetListAsync()).Count,
                TotalBrands       = (await _brandService.TGetListAsync()).Count,
                TotalCategories   = (await _categoryService.TGetListAsync()).Count,
                TotalLocations    = (await _locationService.TGetListAsync()).Count,
                TotalReservations = (await _reservationService.TGetListAsync()).Count,
                PendingReservations = (await _reservationService.TGetReservationsByStatusAsync(ReservationStatus.Pending)).Count,
                UnreadMessages    = await _contactService.TGetUnreadCountAsync(),
                TotalCustomers    = (await _customerService.TGetListAsync()).Count
            };

            return View(model);
        }
    }
}