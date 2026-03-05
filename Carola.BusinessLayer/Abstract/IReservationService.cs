using Carola.EntityLayer.Entities;

namespace Carola.BusinessLayer.Abstract;

public interface IReservationService:IGenericService<Reservation>
{
    public Task TInsertAsync(Reservation entity)
    {
        throw new NotImplementedException();
    }

    public Task TUpdateAsync(Reservation entity)
    {
        throw new NotImplementedException();
    }

    public Task TDeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Reservation>> TGetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Reservation> TGetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}