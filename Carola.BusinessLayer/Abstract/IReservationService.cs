using Carola.DtoLayer.ReservationDtos;
using Carola.EntityLayer.Enums;

namespace Carola.BusinessLayer.Abstract
{
    public interface IReservationService : IGenericService<ResultReservationDto>
    {
        Task<List<ResultReservationDto>> TGetReservationsWithDetailsAsync();
        Task<List<ResultReservationDto>> TGetReservationsByStatusAsync(ReservationStatus status);
        Task TCreateReservationAsync(CreateReservationDto dto);
        Task TUpdateReservationAsync(UpdateReservationDto dto);
        Task TApproveReservationAsync(int reservationId);
        Task TRejectReservationAsync(int reservationId);
        Task<GetByIdReservationDto?> TGetByIdDtoAsync(int id);
    }
}