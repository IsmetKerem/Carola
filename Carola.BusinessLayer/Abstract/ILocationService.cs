using Carola.DtoLayer.LocationDtos;

namespace Carola.BusinessLayer.Abstract
{
    public interface ILocationService : IGenericService<ResultLocationDto>
    {
        Task TCreateLocationAsync(CreateLocationDto dto);
        Task TUpdateLocationAsync(UpdateLocationDto dto);
        Task<GetByIdLocationDto?> TGetByIdDtoAsync(int id);
    }
}