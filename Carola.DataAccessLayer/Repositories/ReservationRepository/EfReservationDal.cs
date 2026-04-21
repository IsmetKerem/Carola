using Carola.DataAccessLayer.Context;
using Carola.DataAccessLayer.Repositories.GenericRepository;
using Carola.EntityLayer.Entities;
using Carola.EntityLayer.Enums;
using Microsoft.EntityFrameworkCore;

namespace Carola.DataAccessLayer.Repositories.ReservationRepository
{
    public class EfReservationDal : GenericRepository<Reservation>, IReservationDal
    {
        private readonly CarolaContext _context;

        public EfReservationDal(CarolaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Reservation>> GetReservationsWithDetailsAsync()
        {
            return await _context.Reservations.AsNoTracking()
                .Include(r => r.Car).ThenInclude(c => c.Brand)
                .Include(r => r.Customer)
                .Include(r => r.PickupLocation)
                .Include(r => r.ReturnLocation)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();
        }

        public async Task<List<Reservation>> GetReservationsByStatusAsync(ReservationStatus status)
        {
            return await _context.Reservations.AsNoTracking()
                .Include(r => r.Car).ThenInclude(c => c.Brand)
                .Include(r => r.Customer)
                .Where(r => r.Status == status)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();
        }

        public async Task UpdateStatusAsync(int reservationId, ReservationStatus status)
        {
            var reservation = await _context.Reservations.FindAsync(reservationId);
            if (reservation != null)
            {
                reservation.Status = status;
                await _context.SaveChangesAsync();
            }
        }
    }
}