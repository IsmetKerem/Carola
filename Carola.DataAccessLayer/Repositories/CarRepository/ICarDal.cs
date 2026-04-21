using Carola.DataAccessLayer.Repositories.GenericRepository;
using Carola.EntityLayer.Entities;

namespace Carola.DataAccessLayer.Repositories.CarRepository
{
    public interface ICarDal : IGenericDal<Car>
    {
        Task<List<Car>> GetCarsWithBrandAndCategoryAsync();
        Task<List<Car>> GetLast6CarsAsync();
        Task<Car?> GetCarByIdWithDetailsAsync(int id);
        Task<List<Car>> GetAvailableCarsBetweenDatesAsync(DateTime start, DateTime end);
    }
}