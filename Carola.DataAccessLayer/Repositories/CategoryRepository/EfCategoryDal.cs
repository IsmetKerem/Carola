using Carola.DataAccessLayer.Context;
using Carola.DataAccessLayer.Repositories.GenericRepository;
using Carola.EntityLayer.Entities;

namespace Carola.DataAccessLayer.Repositories.CategoryRepository
{
    public class EfCategoryDal : GenericRepository<Category>, ICategoryDal
    {
        public EfCategoryDal(CarolaContext context) : base(context) { }
    }
}