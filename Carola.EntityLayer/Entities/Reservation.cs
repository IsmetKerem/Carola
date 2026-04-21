using System;
using Carola.EntityLayer.Enums;

namespace Carola.EntityLayer.Entities
{
    public class Reservation
    {
        public int ReservationId { get; set; }

        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalPrice { get; set; }

        public ReservationStatus Status { get; set; }  
        public DateTime CreatedDate { get; set; }      
        public string Description { get; set; }        

        // Foreign Keys + Navigation
        public int CarId { get; set; }
        public Car Car { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int PickupLocationId { get; set; }
        public Location PickupLocation { get; set; }

        public int ReturnLocationId { get; set; }
        public Location ReturnLocation { get; set; }
    }
}