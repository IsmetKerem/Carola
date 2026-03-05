using Carola.EntityLayer.Entities;

namespace Carola.BusinessLayer.Abstract;

public interface ILocationService:IGenericService<Location>
{
    public Task TInsertAsync(Location entity)
    {
        throw new NotImplementedException();
    }

    public Task TUpdateAsync(Location entity)
    {
        throw new NotImplementedException();
    }

    public Task TDeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Location>> TGetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Location> TGetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}