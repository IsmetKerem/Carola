using Carola.EntityLayer.Entities;

namespace Carola.BusinessLayer.Abstract;

public interface IBrandService:IGenericService<Brand>
{
    public Task TInsertAsync(Brand entity)
    {
        throw new NotImplementedException();
    }

    public Task TUpdateAsync(Brand entity)
    {
        throw new NotImplementedException();
    }

    public Task TDeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Brand>> TGetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Brand> TGetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}