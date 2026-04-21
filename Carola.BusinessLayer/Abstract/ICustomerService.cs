using Carola.DtoLayer.CustomerDtos;

namespace Carola.BusinessLayer.Abstract
{
    public interface ICustomerService : IGenericService<ResultCustomerDto>
    {
        Task<ResultCustomerDto?> TGetCustomerByEmailAsync(string email);
        Task<ResultCustomerDto?> TGetCustomerByLicenseNumberAsync(string licenseNumber);
        Task<int> TCreateCustomerAndReturnIdAsync(CreateCustomerDto dto);
        Task TUpdateCustomerAsync(UpdateCustomerDto dto);
        Task<GetByIdCustomerDto?> TGetByIdDtoAsync(int id);
    }
}