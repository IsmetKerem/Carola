using Carola.EntityLayer.Enums;

namespace Carola.DtoLayer.ReservationDtos
{
    public class GetByIdReservationDto
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
    }
}