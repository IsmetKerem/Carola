using Carola.DtoLayer.VideoDtos;

namespace Carola.BusinessLayer.Abstract
{
    public interface IVideoService : IGenericService<ResultVideoDto>
    {
        Task TCreateVideoAsync(CreateVideoDto dto);
        Task TUpdateVideoAsync(UpdateVideoDto dto);
        Task<GetByIdVideoDto?> TGetByIdDtoAsync(int id);
    }
}