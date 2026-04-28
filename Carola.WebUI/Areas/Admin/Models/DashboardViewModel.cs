namespace Carola.WebUI.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public int TotalCars { get; set; }
        public int TotalReservations { get; set; }
        public int PendingReservations { get; set; }
        public int UnreadMessages { get; set; }
        public int TotalBrands { get; set; }
        public int TotalCategories { get; set; }
        public int TotalLocations { get; set; }
        public int TotalCustomers { get; set; }

        public decimal TotalRevenue { get; set; }            // Approved + Completed toplam
        public decimal MonthRevenue { get; set; }            // Bu ayın ciro toplamı
        public double MonthGrowthPercent { get; set; }       // Geçen aya kıyasla %
        public int NewCustomersThisMonth { get; set; }
        public int ReservationsThisMonth { get; set; }
        public double AverageRating { get; set; }
        public int TotalComments { get; set; }

        public List<MonthlyRevenueDto> MonthlyRevenue { get; set; } = new();

        public ReservationStatusBreakdown StatusBreakdown { get; set; } = new();

        public List<TopCarDto> TopCars { get; set; } = new();

        public List<TopCityDto> TopCities { get; set; } = new();

        public List<BrandStatDto> BrandStats { get; set; } = new();

        public List<CategoryStatDto> CategoryStats { get; set; } = new();

        public List<RecentReservationDto> RecentReservations { get; set; } = new();

        public List<RecentCommentDto> RecentComments { get; set; } = new();
    }

    public class MonthlyRevenueDto
    {
        public string MonthLabel { get; set; } = "";  // "Oca 2026"
        public decimal Revenue { get; set; }
        public int ReservationCount { get; set; }
    }

    public class ReservationStatusBreakdown
    {
        public int Pending { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int Cancelled { get; set; }
        public int Completed { get; set; }
    }

    public class TopCarDto
    {
        public string CarName { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public int RentCount { get; set; }
        public decimal Revenue { get; set; }
    }

    public class TopCityDto
    {
        public string City { get; set; } = "";
        public int ReservationCount { get; set; }
    }

    public class BrandStatDto
    {
        public string BrandName { get; set; } = "";
        public int CarCount { get; set; }
    }

    public class CategoryStatDto
    {
        public string CategoryName { get; set; } = "";
        public int CarCount { get; set; }
    }

    public class RecentReservationDto
    {
        public int ReservationId { get; set; }
        public string CustomerName { get; set; } = "";
        public string CarName { get; set; } = "";
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class RecentCommentDto
    {
        public string UserName { get; set; } = "";
        public string UserImage { get; set; } = "";
        public string CommentText { get; set; } = "";
        public int Rating { get; set; }
        public string CarName { get; set; } = "";
        public DateTime CreatedDate { get; set; }
    }
}