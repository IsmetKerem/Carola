using AutoMapper;
using Carola.BusinessLayer.Abstract;
using Carola.DataAccessLayer.Repositories.SliderRepository;
using Carola.DtoLayer.SliderDtos;
using Carola.EntityLayer.Entities;

namespace Carola.BusinessLayer.Concrete
{
    public class SliderManager : GenericManager<ResultSliderDto, Slider>, ISliderService
    {
        private readonly ISliderDal _sliderDal;
        private readonly IMapper _mapper;

        public SliderManager(ISliderDal sliderDal, IMapper mapper) : base(sliderDal, mapper)
        {
            _sliderDal = sliderDal;
            _mapper = mapper;
        }

        public async Task<List<ResultSliderDto>> TGetActiveSlidersAsync()
        {
            var sliders = await _sliderDal.GetActiveSlidersAsync();
            return _mapper.Map<List<ResultSliderDto>>(sliders);
        }

        public async Task TCreateSliderAsync(CreateSliderDto dto)
        {
            var entity = _mapper.Map<Slider>(dto);
            await _sliderDal.InsertAsync(entity);
        }

        public async Task TUpdateSliderAsync(UpdateSliderDto dto)
        {
            var entity = _mapper.Map<Slider>(dto);
            await _sliderDal.UpdateAsync(entity);
        }

        public async Task<GetByIdSliderDto?> TGetByIdDtoAsync(int id)
        {
            var entity = await _sliderDal.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<GetByIdSliderDto>(entity);
        }
    }
}