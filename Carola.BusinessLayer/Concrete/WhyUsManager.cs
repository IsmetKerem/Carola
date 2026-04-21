using AutoMapper;
using Carola.BusinessLayer.Abstract;
using Carola.DataAccessLayer.Repositories.WhyUsRepository;
using Carola.DtoLayer.WhyUsDtos;
using Carola.EntityLayer.Entities;

namespace Carola.BusinessLayer.Concrete
{
    public class WhyUsManager : GenericManager<ResultWhyUsDto, WhyUs>, IWhyUsService
    {
        private readonly IWhyUsDal _whyUsDal;
        private readonly IMapper _mapper;

        public WhyUsManager(IWhyUsDal whyUsDal, IMapper mapper) : base(whyUsDal, mapper)
        {
            _whyUsDal = whyUsDal;
            _mapper = mapper;
        }

        public async Task TCreateWhyUsAsync(CreateWhyUsDto dto)
        {
            var entity = _mapper.Map<WhyUs>(dto);
            await _whyUsDal.InsertAsync(entity);
        }

        public async Task TUpdateWhyUsAsync(UpdateWhyUsDto dto)
        {
            var entity = _mapper.Map<WhyUs>(dto);
            await _whyUsDal.UpdateAsync(entity);
        }

        public async Task<GetByIdWhyUsDto?> TGetByIdDtoAsync(int id)
        {
            var entity = await _whyUsDal.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<GetByIdWhyUsDto>(entity);
        }
    }
}