namespace Carola.WebUI.Models
{
    public class CompleteBookingDto
    {
        // OCR
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateTime BirthDate { get; set; }
        public string DriverLicenseNumber { get; set; } = "";
        public string DriverLicenseClass { get; set; } = "";
        public DateTime DriverLicenseIssueDate { get; set; }
        public string LicenseImageUrl { get; set; } = "";

        // MANUAL
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";

        // CAR İNFO
        public int CarId { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int PickupLocationId { get; set; }
        public int ReturnLocationId { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Description { get; set; }
    }
}