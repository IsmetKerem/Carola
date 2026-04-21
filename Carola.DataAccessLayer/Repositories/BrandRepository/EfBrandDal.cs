using Carola.DataAccessLayer.Context;
using Carola.DataAccessLayer.Repositories.GenericRepository;
using Carola.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carola.DataAccessLayer.Repositories.BrandRepository
{
    public class EfBrandDal : GenericRepository<Brand>, IBrandDal
    {
        private readonly CarolaContext _context;

        public EfBrandDal(CarolaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Brand>> GetFeaturedBrandsAsync()
        {
            return await _context.Brands
                .AsNoTracking()
                .Where(b => b.Status)
                .ToListAsync();
        }
    }
}