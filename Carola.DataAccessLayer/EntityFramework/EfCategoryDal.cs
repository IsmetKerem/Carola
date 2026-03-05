using Carola.DataAccessLayer.Concrete;
using Carola.DataAccessLayer.Repository;
using Carola.EntityLayer.Entities;

namespace Carola.DataAccessLayer.EntityFramework;

public class EfCategoryDal:GenericRepository<Category>
{
    public EfCategoryDal(CarolaContext context) : base(context)
    {
    }
}