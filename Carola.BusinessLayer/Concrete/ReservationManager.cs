using AutoMapper;
using Carola.BusinessLayer.Abstract;
using Carola.DataAccessLayer.Repositories.ReservationRepository;
using Carola.DtoLayer.ReservationDtos;
using Carola.EntityLayer.Entities;
using Carola.EntityLayer.Enums;

namespace Carola.BusinessLayer.Concrete
{
    public class ReservationManager : GenericManager<ResultReservationDto, Reservation>, IReservationService
    {
        private readonly IReservationDal _reservationDal;
        private readonly IMapper _mapper;

        public ReservationManager(IReservationDal reservationDal, IMapper mapper) : base(reservationDal, mapper)
        {
            _reservationDal = reservationDal;
            _mapper = mapper;
        }

        public async Task<List<ResultReservationDto>> TGetReservationsWithDetailsAsync()
        {
            var list = await _reservationDal.GetReservationsWithDetailsAsync();
            return _mapper.Map<List<ResultReservationDto>>(list);
        }

        public async Task<List<ResultReservationDto>> TGetReservationsByStatusAsync(ReservationStatus status)
        {
            var list = await _reservationDal.GetReservationsByStatusAsync(status);
            return _mapper.Map<List<ResultReservationDto>>(list);
        }

        public async Task TCreateReservationAsync(CreateReservationDto dto)
        {
            var entity = _mapper.Map<Reservation>(dto);
            entity.Status = ReservationStatus.Pending;      
            entity.CreatedDate = DateTime.Now;
            await _reservationDal.InsertAsync(entity);
        }

        public async Task TUpdateReservationAsync(UpdateReservationDto dto)
        {
            var entity = _mapper.Map<Reservation>(dto);
            await _reservationDal.UpdateAsync(entity);
        }

        public async Task TApproveReservationAsync(int reservationId)
        {
            await _reservationDal.UpdateStatusAsync(reservationId, ReservationStatus.Approved);
        }

        public async Task TRejectReservationAsync(int reservationId)
        {
            await _reservationDal.UpdateStatusAsync(reservationId, ReservationStatus.Rejected);
        }

        public async Task<GetByIdReservationDto?> TGetByIdDtoAsync(int id)
        {
            var entity = await _reservationDal.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<GetByIdReservationDto>(entity);
        }
    }
}