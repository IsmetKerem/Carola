using AutoMapper;
using Carola.BusinessLayer.Abstract;
using Carola.DataAccessLayer.Repositories.VideoRepository;
using Carola.DtoLayer.VideoDtos;
using Carola.EntityLayer.Entities;

namespace Carola.BusinessLayer.Concrete
{
    public class VideoManager : GenericManager<ResultVideoDto, Video>, IVideoService
    {
        private readonly IVideoDal _videoDal;
        private readonly IMapper _mapper;

        public VideoManager(IVideoDal videoDal, IMapper mapper) : base(videoDal, mapper)
        {
            _videoDal = videoDal;
            _mapper = mapper;
        }

        public async Task TCreateVideoAsync(CreateVideoDto dto)
        {
            var entity = _mapper.Map<Video>(dto);
            await _videoDal.InsertAsync(entity);
        }

        public async Task TUpdateVideoAsync(UpdateVideoDto dto)
        {
            var entity = _mapper.Map<Video>(dto);
            await _videoDal.UpdateAsync(entity);
        }

        public async Task<GetByIdVideoDto?> TGetByIdDtoAsync(int id)
        {
            var entity = await _videoDal.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<GetByIdVideoDto>(entity);
        }
    }
}