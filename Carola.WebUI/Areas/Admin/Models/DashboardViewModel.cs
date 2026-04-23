namespace Carola.WebUI.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public int TotalCars { get; set; }
        public int TotalBrands { get; set; }
        public int TotalCategories { get; set; }
        public int TotalLocations { get; set; }
        public int TotalReservations { get; set; }
        public int PendingReservations { get; set; }
        public int UnreadMessages { get; set; }
        public int TotalCustomers { get; set; }
    }
}