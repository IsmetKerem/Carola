using Carola.DataAccessLayer.Concrete;
using Carola.DataAccessLayer.Repository;
using Carola.EntityLayer.Entities;

namespace Carola.DataAccessLayer.EntityFramework;

public class EfBrandDal:GenericRepository<Brand>
{
    public EfBrandDal(CarolaContext context) : base(context)
    {
    }
}