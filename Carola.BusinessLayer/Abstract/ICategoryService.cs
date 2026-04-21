using Carola.BusinessLayer.Abstract;
using Carola.DtoLayer.CategoryDtos;

namespace Carola.BusinessLayer.Abstract
{
    public interface ICategoryService : IGenericService<ResultCategoryDto>
    {
        Task TCreateCategoryAsync(CreateCategoryDto dto);
        Task TUpdateCategoryAsync(UpdateCategoryDto dto);
        Task<GetByIdCategoryDto?> TGetByIdDtoAsync(int id);
    }
}