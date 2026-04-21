using AutoMapper;
using Carola.BusinessLayer.Abstract;
using Carola.DataAccessLayer.Repositories.CarRepository;
using Carola.DtoLayer.CarDtos;
using Carola.EntityLayer.Entities;

namespace Carola.BusinessLayer.Concrete
{
    public class CarManager : GenericManager<ResultCarDto, Car>, ICarService
    {
        private readonly ICarDal _carDal;
        private readonly IMapper _mapper;

        public CarManager(ICarDal carDal, IMapper mapper) : base(carDal, mapper)
        {
            _carDal = carDal;
            _mapper = mapper;
        }

        public async Task<List<ResultCarDto>> TGetCarsWithBrandAndCategoryAsync()
        {
            var cars = await _carDal.GetCarsWithBrandAndCategoryAsync();
            return _mapper.Map<List<ResultCarDto>>(cars);
        }

        public async Task<List<ResultCarDto>> TGetLast6CarsAsync()
        {
            var cars = await _carDal.GetLast6CarsAsync();
            return _mapper.Map<List<ResultCarDto>>(cars);
        }

        public async Task<GetByIdCarDto?> TGetCarByIdWithDetailsAsync(int id)
        {
            var car = await _carDal.GetCarByIdWithDetailsAsync(id);
            return car == null ? null : _mapper.Map<GetByIdCarDto>(car);
        }

        public async Task<List<ResultCarDto>> TGetAvailableCarsBetweenDatesAsync(DateTime start, DateTime end)
        {
            var cars = await _carDal.GetAvailableCarsBetweenDatesAsync(start, end);
            return _mapper.Map<List<ResultCarDto>>(cars);
        }

        public async Task TCreateCarAsync(CreateCarDto dto)
        {
            var entity = _mapper.Map<Car>(dto);
            await _carDal.InsertAsync(entity);
        }

        public async Task TUpdateCarAsync(UpdateCarDto dto)
        {
            var entity = _mapper.Map<Car>(dto);
            await _carDal.UpdateAsync(entity);
        }
    }
}