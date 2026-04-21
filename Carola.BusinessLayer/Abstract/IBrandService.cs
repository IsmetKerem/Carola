using Carola.DtoLayer.BrandDtos;

namespace Carola.BusinessLayer.Abstract
{
    public interface IBrandService : IGenericService<ResultBrandDto>
    {
        Task<List<ResultBrandDto>> TGetFeaturedBrandsAsync();
        Task TCreateBrandAsync(CreateBrandDto dto);
        Task TUpdateBrandAsync(UpdateBrandDto dto);
        Task<GetByIdBrandDto?> TGetByIdDtoAsync(int id);
    }
}