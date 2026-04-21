using AutoMapper;
using Carola.BusinessLayer.Abstract;
using Carola.DataAccessLayer.Repositories.CustomerRepository;
using Carola.DtoLayer.CustomerDtos;
using Carola.EntityLayer.Entities;

namespace Carola.BusinessLayer.Concrete
{
    public class CustomerManager : GenericManager<ResultCustomerDto, Customer>, ICustomerService
    {
        private readonly ICustomerDal _customerDal;
        private readonly IMapper _mapper;

        public CustomerManager(ICustomerDal customerDal, IMapper mapper) : base(customerDal, mapper)
        {
            _customerDal = customerDal;
            _mapper = mapper;
        }

        public async Task<ResultCustomerDto?> TGetCustomerByEmailAsync(string email)
        {
            var customer = await _customerDal.GetCustomerByEmailAsync(email);
            return customer == null ? null : _mapper.Map<ResultCustomerDto>(customer);
        }

        public async Task<ResultCustomerDto?> TGetCustomerByLicenseNumberAsync(string licenseNumber)
        {
            var customer = await _customerDal.GetCustomerByLicenseNumberAsync(licenseNumber);
            return customer == null ? null : _mapper.Map<ResultCustomerDto>(customer);
        }

        // Booking akışında Reservation için CustomerId lazım — bu yüzden Id döndürüyoruz
        public async Task<int> TCreateCustomerAndReturnIdAsync(CreateCustomerDto dto)
        {
            var entity = _mapper.Map<Customer>(dto);
            await _customerDal.InsertAsync(entity);
            return entity.CustomerId; // EF Core InsertAsync sonrası Id otomatik set olur
        }

        public async Task TUpdateCustomerAsync(UpdateCustomerDto dto)
        {
            var entity = _mapper.Map<Customer>(dto);
            await _customerDal.UpdateAsync(entity);
        }

        public async Task<GetByIdCustomerDto?> TGetByIdDtoAsync(int id)
        {
            var entity = await _customerDal.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<GetByIdCustomerDto>(entity);
        }
    }
}