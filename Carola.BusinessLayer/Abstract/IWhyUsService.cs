using Carola.DtoLayer.WhyUsDtos;

namespace Carola.BusinessLayer.Abstract
{
    public interface IWhyUsService : IGenericService<ResultWhyUsDto>
    {
        Task TCreateWhyUsAsync(CreateWhyUsDto dto);
        Task TUpdateWhyUsAsync(UpdateWhyUsDto dto);
        Task<GetByIdWhyUsDto?> TGetByIdDtoAsync(int id);
    }
}