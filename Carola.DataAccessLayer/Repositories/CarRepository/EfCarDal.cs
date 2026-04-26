using Carola.DataAccessLayer.Context;
using Carola.DataAccessLayer.Repositories.GenericRepository;
using Carola.DtoLayer.CarDtos;
using Carola.DtoLayer.Common;
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

        public async Task<PagedResult<Car>> GetFilteredCarsAsync(CarFilterDto filter)
        {
            var query = _context.Cars
                .AsNoTracking()
                .Include(c => c.Brand)
                .Include(c => c.Category)
                .Where(c => c.IsAvailable);

            if (filter.PickupDate.HasValue && filter.ReturnDate.HasValue)
            {
                var pickup = filter.PickupDate.Value;
                var ret = filter.ReturnDate.Value;

                query = query.Where(c => !c.Reservations.Any(r =>
                    (r.Status == EntityLayer.Enums.ReservationStatus.Pending ||
                     r.Status == EntityLayer.Enums.ReservationStatus.Approved) &&
                    r.PickupDate < ret &&
                    r.ReturnDate > pickup));
            }

            if (filter.BrandId.HasValue && filter.BrandId.Value > 0)
                query = query.Where(c => c.BrandId == filter.BrandId.Value);

            if (filter.CategoryId.HasValue && filter.CategoryId.Value > 0)
                query = query.Where(c => c.CategoryId == filter.CategoryId.Value);

            if (!string.IsNullOrWhiteSpace(filter.FuelType))
                query = query.Where(c => c.FuelType == filter.FuelType);

            if (!string.IsNullOrWhiteSpace(filter.TransmissionType))
                query = query.Where(c => c.TransmissionType == filter.TransmissionType);

            if (filter.SeatCount.HasValue && filter.SeatCount.Value > 0)
                query = query.Where(c => c.SeatCount >= filter.SeatCount.Value);

            if (filter.MinPrice.HasValue)
                query = query.Where(c => c.DailyPrice >= filter.MinPrice.Value);
            if (filter.MaxPrice.HasValue)
                query = query.Where(c => c.DailyPrice <= filter.MaxPrice.Value);

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var term = filter.SearchTerm.Trim().ToLower();
                query = query.Where(c =>
                    c.Model.ToLower().Contains(term) ||
                    c.Brand.BrandName.ToLower().Contains(term));
            }

            query = filter.SortBy switch
            {
                "price_asc" => query.OrderBy(c => c.DailyPrice),
                "price_desc" => query.OrderByDescending(c => c.DailyPrice),
                "oldest" => query.OrderBy(c => c.CarId),
                _ => query.OrderByDescending(c => c.CarId) // newest (default)
            };

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PagedResult<Car>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }
        public async Task<decimal> GetMaxDailyPriceAsync()
        {
            return await _context.Cars
                .Where(c => c.IsAvailable)
                .MaxAsync(c => (decimal?)c.DailyPrice) ?? 10000m;
        }
    }
}