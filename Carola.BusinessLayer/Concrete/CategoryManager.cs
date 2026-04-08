using Carola.BusinessLayer.Abstract;
using Carola.DataAccessLayer.Abstract;
using Carola.EntityLayer.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.BusinessLayer.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public async Task TDeleteAsync(int id)
        {
            await _categoryDal.DeleteAsync(id);
        }
        public async Task<List<Category>> TGetAllAsync()
        {
            return await _categoryDal.GetAllAsync();
        }
        public async Task<Category> TGetByIdAsync(int id)
        {
            return await _categoryDal.GetByIdAsync(id);
        }
        public async Task TInsertAsync(Category entity)
        {
            await _categoryDal.InsertAsync(entity);
        }
        public async Task TUpdateAsync(Category entity)
        {
            await _categoryDal.UpdateAsync(entity);
        }
    }
}
