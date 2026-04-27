using Carola.EntityLayer.Enums;

namespace Carola.DtoLayer.ReservationDtos
{
    public class ResultReservationDto
    {
        public int ReservationId { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalPrice { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public int CarId { get; set; }
        public int CustomerId { get; set; }
        public int PickupLocationId { get; set; }
        public int ReturnLocationId { get; set; }

        public string CarModel { get; set; }
        public string BrandName { get; set; }
        public string CustomerFullName { get; set; }
        public string PickupLocationName { get; set; }
        public string ReturnLocationName { get; set; }

        public string? CustomerFirstName { get; set; }
        public string? CustomerLastName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
    }
}