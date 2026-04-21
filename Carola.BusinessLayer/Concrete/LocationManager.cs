using AutoMapper;
using Carola.BusinessLayer.Abstract;
using Carola.DataAccessLayer.Repositories.LocationRepository;
using Carola.DtoLayer.LocationDtos;
using Carola.EntityLayer.Entities;

namespace Carola.BusinessLayer.Concrete
{
    public class LocationManager : GenericManager<ResultLocationDto, Location>, ILocationService
    {
        private readonly ILocationDal _locationDal;
        private readonly IMapper _mapper;

        public LocationManager(ILocationDal locationDal, IMapper mapper) : base(locationDal, mapper)
        {
            _locationDal = locationDal;
            _mapper = mapper;
        }

        public async Task TCreateLocationAsync(CreateLocationDto dto)
        {
            var entity = _mapper.Map<Location>(dto);
            await _locationDal.InsertAsync(entity);
        }

        public async Task TUpdateLocationAsync(UpdateLocationDto dto)
        {
            var entity = _mapper.Map<Location>(dto);
            await _locationDal.UpdateAsync(entity);
        }

        public async Task<GetByIdLocationDto?> TGetByIdDtoAsync(int id)
        {
            var entity = await _locationDal.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<GetByIdLocationDto>(entity);
        }
    }
}