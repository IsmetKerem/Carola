using Carola.DataAccessLayer.Repositories.GenericRepository;
using Carola.EntityLayer.Entities;

namespace Carola.DataAccessLayer.Repositories.SliderRepository
{
    public interface ISliderDal : IGenericDal<Slider>
    {
        Task<List<Slider>> GetActiveSlidersAsync(); // Status = true
    }
}