using Carola.DtoLayer.BrandDtos;
using Carola.DtoLayer.CarDtos;
using Carola.DtoLayer.CategoryDtos;
using Carola.DtoLayer.LocationDtos;
using Carola.DtoLayer.SliderDtos;
using Carola.DtoLayer.WhyUsDtos;

namespace Carola.WebUI.Models
{
    public class HomeViewModel
    {
        public List<ResultSliderDto> Sliders { get; set; } = new();
        public List<ResultCategoryDto> Categories { get; set; } = new();
        public List<ResultWhyUsDto> WhyUsItems { get; set; } = new();
        public List<ResultBrandDto> FeaturedBrands { get; set; } = new();
        public List<ResultBrandDto> AllBrands { get; set; } = new();  
        public List<ResultCarDto> FeaturedCars { get; set; } = new();  
        public List<ResultLocationDto> Locations { get; set; } = new();
    }
}