namespace Carola.EntityLayer.Entities;

public class Reservation
{
    public int ReservationId { get; set; }
    public int CarId { get; set; }
    public int CustomerId { get; set; }
    public DateTime PickupDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public int PickupLocationId { get; set; }
    public int ReturnLocationId { get; set; }
    public decimal TotalPrice { get; set; }
    public string  ReservationStatus { get; set; }
    public string  Description { get; set; }
}