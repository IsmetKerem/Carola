using Carola.DataAccessLayer.Repositories.GenericRepository;
using Carola.EntityLayer.Entities;

namespace Carola.DataAccessLayer.Repositories.CustomerRepository
{
    public interface ICustomerDal : IGenericDal<Customer>
    {
        Task<Customer?> GetCustomerByEmailAsync(string email);
        Task<Customer?> GetCustomerByLicenseNumberAsync(string licenseNumber);
    }
}