using Carola.DtoLayer.SliderDtos;

namespace Carola.BusinessLayer.Abstract
{
    public interface ISliderService : IGenericService<ResultSliderDto>
    {
        Task<List<ResultSliderDto>> TGetActiveSlidersAsync();
        Task TCreateSliderAsync(CreateSliderDto dto);
        Task TUpdateSliderAsync(UpdateSliderDto dto);
        Task<GetByIdSliderDto?> TGetByIdDtoAsync(int id);
    }
}