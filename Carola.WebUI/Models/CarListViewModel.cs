using Carola.DtoLayer.BrandDtos;
using Carola.DtoLayer.CarDtos;
using Carola.DtoLayer.CategoryDtos;
using Carola.DtoLayer.Common;

namespace Carola.WebUI.Models
{
    public class CarsListViewModel
    {
        public PagedResult<ResultCarDto> CarsPage { get; set; } = new();
        public CarFilterDto Filter { get; set; } = new();

        public List<ResultBrandDto> Brands { get; set; } = new();
        public List<ResultCategoryDto> Categories { get; set; } = new();
        public decimal MaxAvailablePrice { get; set; }

        public List<string> FuelTypes { get; set; } = new()
        {
            "Benzin", "Dizel", "LPG", "Hybrid", "Elektrik"
        };

        public List<string> TransmissionTypes { get; set; } = new()
        {
            "Manuel", "Otomatik", "Yarı Otomatik"
        };
    }
}