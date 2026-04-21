using Carola.DtoLayer.GalleryDtos;

namespace Carola.BusinessLayer.Abstract
{
    public interface IGalleryService : IGenericService<ResultGalleryDto>
    {
        Task TCreateGalleryAsync(CreateGalleryDto dto);
        Task TUpdateGalleryAsync(UpdateGalleryDto dto);
        Task<GetByIdGalleryDto?> TGetByIdDtoAsync(int id);
    }
}