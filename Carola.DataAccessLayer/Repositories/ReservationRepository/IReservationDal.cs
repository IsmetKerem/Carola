using Carola.DataAccessLayer.Repositories.GenericRepository;
using Carola.EntityLayer.Entities;
using Carola.EntityLayer.Enums;

namespace Carola.DataAccessLayer.Repositories.ReservationRepository
{
    public interface IReservationDal : IGenericDal<Reservation>
    {
        Task<List<Reservation>> GetReservationsWithDetailsAsync();
        Task<List<Reservation>> GetReservationsByStatusAsync(ReservationStatus status);
        Task UpdateStatusAsync(int reservationId, ReservationStatus status);
    }
}