namespace Carola.DtoLayer.CarDtos
{
    public class CreateCarDto
    {
        public string Model { get; set; }
        public int ModelYear { get; set; }
        public string PlateNumber { get; set; }
        public decimal DailyPrice { get; set; }
        public int SeatCount { get; set; }
        public int LuggageCapacity { get; set; }
        public int Mileage { get; set; }
        public bool IsAvailable { get; set; }
        public string FuelType { get; set; }
        public string TransmissionType { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
    }
}