namespace Carola.DtoLayer.CarDtos
{
    public class CarFilterDto
    {
        public DateTime? PickupDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public int? LocationId { get; set; }
        public string? FuelType { get; set; }
        public string? TransmissionType { get; set; }
        public int? SeatCount { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public string? SearchTerm { get; set; }

        public string? SortBy { get; set; } 

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 6;
    }
}