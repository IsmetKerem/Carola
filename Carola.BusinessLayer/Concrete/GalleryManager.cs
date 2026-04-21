using AutoMapper;
using Carola.BusinessLayer.Abstract;
using Carola.DataAccessLayer.Repositories.GalleryRepository;
using Carola.DtoLayer.GalleryDtos;
using Carola.EntityLayer.Entities;

namespace Carola.BusinessLayer.Concrete
{
    public class GalleryManager : GenericManager<ResultGalleryDto, Gallery>, IGalleryService
    {
        private readonly IGalleryDal _galleryDal;
        private readonly IMapper _mapper;

        public GalleryManager(IGalleryDal galleryDal, IMapper mapper) : base(galleryDal, mapper)
        {
            _galleryDal = galleryDal;
            _mapper = mapper;
        }

        public async Task TCreateGalleryAsync(CreateGalleryDto dto)
        {
            var entity = _mapper.Map<Gallery>(dto);
            await _galleryDal.InsertAsync(entity);
        }

        public async Task TUpdateGalleryAsync(UpdateGalleryDto dto)
        {
            var entity = _mapper.Map<Gallery>(dto);
            await _galleryDal.UpdateAsync(entity);
        }

        public async Task<GetByIdGalleryDto?> TGetByIdDtoAsync(int id)
        {
            var entity = await _galleryDal.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<GetByIdGalleryDto>(entity);
        }
    }
}