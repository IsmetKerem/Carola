using Carola.DtoLayer.CarDtos;
using Carola.DtoLayer.Common;

namespace Carola.BusinessLayer.Abstract
{
    public interface ICarService : IGenericService<ResultCarDto>
    {
        Task<List<ResultCarDto>> TGetCarsWithBrandAndCategoryAsync();
        Task<List<ResultCarDto>> TGetLast6CarsAsync();
        Task<GetByIdCarDto?> TGetCarByIdWithDetailsAsync(int id);
        Task<List<ResultCarDto>> TGetAvailableCarsBetweenDatesAsync(DateTime start, DateTime end);
        Task TCreateCarAsync(CreateCarDto dto);
        Task TUpdateCarAsync(UpdateCarDto dto);
        Task<PagedResult<ResultCarDto>> TGetFilteredCarsAsync(CarFilterDto filter);
        Task<decimal> TGetMaxDailyPriceAsync();
    }
}