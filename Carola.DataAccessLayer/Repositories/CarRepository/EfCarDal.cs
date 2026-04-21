using Carola.DataAccessLayer.Context;
using Carola.DataAccessLayer.Repositories.GenericRepository;
using Carola.EntityLayer.Entities;
using Carola.EntityLayer.Enums;
using Microsoft.EntityFrameworkCore;

namespace Carola.DataAccessLayer.Repositories.CarRepository
{
    public class EfCarDal : GenericRepository<Car>, ICarDal
    {
        private readonly CarolaContext _context;

        public EfCarDal(CarolaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Car>> GetCarsWithBrandAndCategoryAsync()
        {
            return await _context.Cars.AsNoTracking()
                .Include(c => c.Brand)
                .Include(c => c.Category)
                .ToListAsync();
        }

        public async Task<List<Car>> GetLast6CarsAsync()
        {
            return await _context.Cars.AsNoTracking()
                .Include(c => c.Brand)
                .Include(c => c.Category)
                .Where(c => c.IsAvailable)
                .OrderByDescending(c => c.CarId)
                .Take(6)
                .ToListAsync();
        }

        public async Task<Car?> GetCarByIdWithDetailsAsync(int id)
        {
            return await _context.Cars.AsNoTracking()
                .Include(c => c.Brand)
                .Include(c => c.Category)
                .Include(c => c.Comments)
                .FirstOrDefaultAsync(c => c.CarId == id);
        }

        // Tarih bazlı çakışma kontrolü — core business logic
        public async Task<List<Car>> GetAvailableCarsBetweenDatesAsync(DateTime start, DateTime end)
        {
            return await _context.Cars.AsNoTracking()
                .Include(c => c.Brand)
                .Include(c => c.Category)
                .Where(c => c.IsAvailable && !c.Reservations.Any(r =>
                    (r.Status == ReservationStatus.Pending || r.Status == ReservationStatus.Approved)
                    && r.PickupDate < end
                    && r.ReturnDate > start))
                .ToListAsync();
        }
    }
}