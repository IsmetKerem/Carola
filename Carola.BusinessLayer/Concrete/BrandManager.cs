using AutoMapper;
using Carola.BusinessLayer.Abstract;
using Carola.DataAccessLayer.Repositories.BrandRepository;
using Carola.DtoLayer.BrandDtos;
using Carola.EntityLayer.Entities;

namespace Carola.BusinessLayer.Concrete
{
    public class BrandManager : GenericManager<ResultBrandDto, Brand>, IBrandService
    {
        private readonly IBrandDal _brandDal;
        private readonly IMapper _mapper;

        public BrandManager(IBrandDal brandDal, IMapper mapper) : base(brandDal, mapper)
        {
            _brandDal = brandDal;
            _mapper = mapper;
        }

        public async Task<List<ResultBrandDto>> TGetFeaturedBrandsAsync()
        {
            var brands = await _brandDal.GetFeaturedBrandsAsync();
            return _mapper.Map<List<ResultBrandDto>>(brands);
        }

        public async Task TCreateBrandAsync(CreateBrandDto dto)
        {
            var entity = _mapper.Map<Brand>(dto);
            await _brandDal.InsertAsync(entity);
        }

        public async Task TUpdateBrandAsync(UpdateBrandDto dto)
        {
            var entity = _mapper.Map<Brand>(dto);
            await _brandDal.UpdateAsync(entity);
        }

        public async Task<GetByIdBrandDto?> TGetByIdDtoAsync(int id)
        {
            var brand = await _brandDal.GetByIdAsync(id);
            return brand == null ? null : _mapper.Map<GetByIdBrandDto>(brand);
        }
    }
}