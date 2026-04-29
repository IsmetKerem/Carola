namespace Carola.DtoLayer.ChatDtos
{
    public class ChatResponseDto
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }

        public string ReplyText { get; set; } = "";

        public List<RecommendedCarDto> RecommendedCars { get; set; } = new();
    }

    public class RecommendedCarDto
    {
        public int CarId { get; set; }
        public string BrandName { get; set; } = "";
        public string Model { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public string FuelType { get; set; } = "";
        public string TransmissionType { get; set; } = "";
        public int SeatCount { get; set; }
        public decimal DailyPrice { get; set; }
        public string ImageUrl { get; set; } = "";
        public string Reason { get; set; } = ""; 
    }
}