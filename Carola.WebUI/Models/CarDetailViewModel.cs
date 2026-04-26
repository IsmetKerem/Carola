using Carola.DtoLayer.CarDtos;
using Carola.DtoLayer.CommentDtos;
using Carola.DtoLayer.LocationDtos;

namespace Carola.WebUI.Models
{
    public class CarDetailViewModel
    {
        public GetByIdCarDto Car { get; set; } = null!;
        public List<ResultCommentDto> Comments { get; set; } = new();
        public List<ResultCarDto> RelatedCars { get; set; } = new(); 
        public List<ResultLocationDto> Locations { get; set; } = new(); 

        // Hesaplanan değerler
        public double AverageRating => Comments.Any() ? Comments.Average(c => c.Rating) : 0;
        public int CommentCount => Comments.Count;
    }
}