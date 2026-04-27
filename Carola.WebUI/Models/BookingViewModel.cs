using Carola.DtoLayer.CarDtos;
using Carola.DtoLayer.LocationDtos;

namespace Carola.WebUI.Models
{
    
    public class BookingViewModel
    {
        
        public int CarId { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int PickupLocationId { get; set; }
        public int ReturnLocationId { get; set; }

        
        public GetByIdCarDto Car { get; set; } = null!;
        public ResultLocationDto PickupLocation { get; set; } = null!;
        public ResultLocationDto ReturnLocation { get; set; } = null!;

        
        public int TotalDays => (ReturnDate - PickupDate).Days;
        public decimal SubTotal => Car.DailyPrice * TotalDays;
        public decimal Tax => SubTotal * 0.18m;       
        public decimal GrandTotal => SubTotal + Tax;
    }
}