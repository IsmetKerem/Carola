using Carola.EntityLayer.Entities;

namespace Carola.BusinessLayer.Abstract;

public interface ICarService:IGenericService<Car>
{
    public Task TInsertAsync(Car entity)
    {
        throw new NotImplementedException();
    }

    public Task TUpdateAsync(Car entity)
    {
        throw new NotImplementedException();
    }

    public Task TDeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Car>> TGetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Car> TGetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}