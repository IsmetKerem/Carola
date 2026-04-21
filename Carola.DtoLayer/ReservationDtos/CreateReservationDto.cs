namespace Carola.DtoLayer.ReservationDtos
{
    public class CreateReservationDto
    {
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Description { get; set; }
        public int CarId { get; set; }
        public int CustomerId { get; set; }
        public int PickupLocationId { get; set; }
        public int ReturnLocationId { get; set; }
    }
}