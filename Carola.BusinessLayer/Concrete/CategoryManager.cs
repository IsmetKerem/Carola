using Carola.BusinessLayer.Abstract;
using Carola.DataAccessLayer.Abstract;
using Carola.EntityLayer.Entities;

namespace Carola.BusinessLayer.Concrete;

public class CategoryManager:ICategoryService
{
    private readonly ICategoryDal _categoryDal;

    public CategoryManager(ICategoryDal categoryDal)
    {
        _categoryDal = categoryDal;
    }

    public Task TInsertAsync(Category entity)
    {
        throw new NotImplementedException();
    }

    public Task TUpdateAsync(Category entity)
    {
        throw new NotImplementedException();
    }

    public async Task TDeleteAsync(int id)
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