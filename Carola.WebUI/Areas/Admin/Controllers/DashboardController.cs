using Carola.BusinessLayer.Abstract;
using Carola.EntityLayer.Enums;
using Carola.WebUI.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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
        private readonly ICustomerService _customerService;
        private readonly IReservationService _reservationService;
        private readonly IContactService _contactService;
        private readonly ICommentService _commentService;

        public DashboardController(
            ICarService carService,
            IBrandService brandService,
            ICategoryService categoryService,
            ILocationService locationService,
            ICustomerService customerService,
            IReservationService reservationService,
            IContactService contactService,
            ICommentService commentService)
        {
            _carService = carService;
            _brandService = brandService;
            _categoryService = categoryService;
            _locationService = locationService;
            _customerService = customerService;
            _reservationService = reservationService;
            _contactService = contactService;
            _commentService = commentService;
        }

        public async Task<IActionResult> Index()
        {
            var cars         = await _carService.TGetListAsync();
            var brands       = await _brandService.TGetListAsync();
            var categories   = await _categoryService.TGetListAsync();
            var locations    = await _locationService.TGetListAsync();
            var customers    = await _customerService.TGetListAsync();
            var reservations = await _reservationService.TGetReservationsWithDetailsAsync();
            var contacts     = await _contactService.TGetListAsync();
            var comments     = await _commentService.TGetListAsync();

            var now    = DateTime.Now;
            var monthStart    = new DateTime(now.Year, now.Month, 1);
            var lastMonthStart = monthStart.AddMonths(-1);

            decimal totalRevenue = reservations
                .Where(r => r.Status == ReservationStatus.Approved || r.Status == ReservationStatus.Completed)
                .Sum(r => r.TotalPrice);

            decimal monthRevenue = reservations
                .Where(r => (r.Status == ReservationStatus.Approved || r.Status == ReservationStatus.Completed)
                            && r.CreatedDate >= monthStart)
                .Sum(r => r.TotalPrice);

            decimal lastMonthRevenue = reservations
                .Where(r => (r.Status == ReservationStatus.Approved || r.Status == ReservationStatus.Completed)
                            && r.CreatedDate >= lastMonthStart && r.CreatedDate < monthStart)
                .Sum(r => r.TotalPrice);

            double growthPercent = lastMonthRevenue > 0
                ? (double)((monthRevenue - lastMonthRevenue) / lastMonthRevenue) * 100
                : 0;

            var monthlyRevenue = new List<MonthlyRevenueDto>();
            var trCulture = new CultureInfo("tr-TR");
            for (int i = 5; i >= 0; i--)
            {
                var mStart = monthStart.AddMonths(-i);
                var mEnd = mStart.AddMonths(1);
                var monthRezs = reservations
                    .Where(r => (r.Status == ReservationStatus.Approved || r.Status == ReservationStatus.Completed)
                                && r.CreatedDate >= mStart && r.CreatedDate < mEnd)
                    .ToList();
                monthlyRevenue.Add(new MonthlyRevenueDto
                {
                    MonthLabel = mStart.ToString("MMM yyyy", trCulture),
                    Revenue = monthRezs.Sum(r => r.TotalPrice),
                    ReservationCount = monthRezs.Count
                });
            }

            var statusBreakdown = new ReservationStatusBreakdown
            {
                Pending   = reservations.Count(r => r.Status == ReservationStatus.Pending),
                Approved  = reservations.Count(r => r.Status == ReservationStatus.Approved),
                Rejected  = reservations.Count(r => r.Status == ReservationStatus.Rejected),
                Cancelled = reservations.Count(r => r.Status == ReservationStatus.Cancelled),
                Completed = reservations.Count(r => r.Status == ReservationStatus.Completed)
            };

            var topCars = reservations
                .Where(r => r.Status != ReservationStatus.Rejected && r.Status != ReservationStatus.Cancelled)
                .GroupBy(r => r.CarId)
                .Select(g =>
                {
                    var car = cars.FirstOrDefault(c => c.CarId == g.Key);
                    return new TopCarDto
                    {
                        CarName = car != null ? $"{car.BrandName} {car.Model}" : "Bilinmeyen",
                        ImageUrl = car?.ImageUrl ?? "",
                        RentCount = g.Count(),
                        Revenue = g.Sum(r => r.TotalPrice)
                    };
                })
                .OrderByDescending(x => x.RentCount)
                .Take(5)
                .ToList();

            var topCities = reservations
                .GroupBy(r => r.PickupLocationName ?? "Bilinmeyen")
                .Select(g => new TopCityDto
                {
                    City = locations.FirstOrDefault(l => l.LocationName == g.Key)?.City ?? g.Key,
                    ReservationCount = g.Count()
                })
                .GroupBy(x => x.City)
                .Select(g => new TopCityDto
                {
                    City = g.Key,
                    ReservationCount = g.Sum(x => x.ReservationCount)
                })
                .OrderByDescending(x => x.ReservationCount)
                .Take(7)
                .ToList();

            var brandStats = cars
                .GroupBy(c => c.BrandName)
                .Select(g => new BrandStatDto { BrandName = g.Key, CarCount = g.Count() })
                .OrderByDescending(x => x.CarCount)
                .Take(10)
                .ToList();

            var categoryStats = cars
                .GroupBy(c => c.CategoryName)
                .Select(g => new CategoryStatDto { CategoryName = g.Key, CarCount = g.Count() })
                .OrderByDescending(x => x.CarCount)
                .ToList();

            var recentReservations = reservations
                .OrderByDescending(r => r.CreatedDate)
                .Take(10)
                .Select(r => new RecentReservationDto
                {
                    ReservationId = r.ReservationId,
                    CustomerName = r.CustomerFullName ?? "—",
                    CarName = $"{r.BrandName} {r.CarModel}",
                    PickupDate = r.PickupDate,
                    ReturnDate = r.ReturnDate,
                    TotalPrice = r.TotalPrice,
                    Status = (int)r.Status,
                    CreatedDate = r.CreatedDate
                }).ToList();

            var recentComments = comments
                .OrderByDescending(c => c.CreatedDate)
                .Take(5)
                .Select(c =>
                {
                    var car = cars.FirstOrDefault(x => x.CarId == c.CarId);
                    return new RecentCommentDto
                    {
                        UserName = c.UserName,
                        UserImage = c.UserImage,
                        CommentText = c.CommentText.Length > 120
                            ? c.CommentText.Substring(0, 120) + "..."
                            : c.CommentText,
                        Rating = c.Rating,
                        CarName = car != null ? $"{car.BrandName} {car.Model}" : "—",
                        CreatedDate = c.CreatedDate
                    };
                }).ToList();

            var vm = new DashboardViewModel
            {
                TotalCars = cars.Count,
                TotalReservations = reservations.Count,
                PendingReservations = statusBreakdown.Pending,
                UnreadMessages = contacts.Count(c => !c.IsRead),
                TotalBrands = brands.Count,
                TotalCategories = categories.Count,
                TotalLocations = locations.Count,
                TotalCustomers = customers.Count,

                TotalRevenue = totalRevenue,
                MonthRevenue = monthRevenue,
                MonthGrowthPercent = growthPercent,
                NewCustomersThisMonth = customers.Count, 
                ReservationsThisMonth = reservations.Count(r => r.CreatedDate >= monthStart),
                AverageRating = comments.Any() ? Math.Round(comments.Average(c => c.Rating), 2) : 0,
                TotalComments = comments.Count,

                MonthlyRevenue = monthlyRevenue,
                StatusBreakdown = statusBreakdown,
                TopCars = topCars,
                TopCities = topCities,
                BrandStats = brandStats,
                CategoryStats = categoryStats,
                RecentReservations = recentReservations,
                RecentComments = recentComments
            };

            return View(vm);
        }
    }
}