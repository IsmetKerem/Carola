using Carola.EntityLayer.Entities;

namespace Carola.BusinessLayer.Abstract;

public interface ICategoryService:IGenericService<Category>
{
    public Task TInsertAsync(Category entity)
    {
        throw new NotImplementedException();
    }

    public Task TUpdateAsync(Category entity)
    {
        throw new NotImplementedException();
    }

    public Task TDeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Category>> TGetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Category> TGetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}