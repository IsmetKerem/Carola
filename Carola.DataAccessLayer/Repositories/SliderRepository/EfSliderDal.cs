using Carola.DataAccessLayer.Context;
using Carola.DataAccessLayer.Repositories.GenericRepository;
using Carola.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carola.DataAccessLayer.Repositories.SliderRepository
{
    public class EfSliderDal : GenericRepository<Slider>, ISliderDal
    {
        private readonly CarolaContext _context;

        public EfSliderDal(CarolaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Slider>> GetActiveSlidersAsync()
        {
            return await _context.Sliders.AsNoTracking()
                .Where(s => s.Status)
                .ToListAsync();
        }
    }
}