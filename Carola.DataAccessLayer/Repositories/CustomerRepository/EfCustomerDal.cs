using Carola.DataAccessLayer.Context;
using Carola.DataAccessLayer.Repositories.GenericRepository;
using Carola.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carola.DataAccessLayer.Repositories.CustomerRepository
{
    public class EfCustomerDal : GenericRepository<Customer>, ICustomerDal
    {
        private readonly CarolaContext _context;

        public EfCustomerDal(CarolaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            return await _context.Customers.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Customer?> GetCustomerByLicenseNumberAsync(string licenseNumber)
        {
            return await _context.Customers.AsNoTracking()
                .FirstOrDefaultAsync(c => c.DriverLicenseNumber == licenseNumber);
        }
    }
}