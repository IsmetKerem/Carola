namespace Carola.WebUI.Models
{
    public class ReservationApprovedMailModel
    {
        public string CustomerFullName { get; set; } = "";
        public string CarBrandModel { get; set; } = "";
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int TotalDays => (ReturnDate - PickupDate).Days;
        public string PickupLocation { get; set; } = "";
        public string ReturnLocation { get; set; } = "";
        public decimal TotalPrice { get; set; }
        public string CouponCode { get; set; } = "";
    }
}