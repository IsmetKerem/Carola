using Carola.DataAccessLayer.Concrete;
using Carola.DataAccessLayer.Repository;
using Carola.EntityLayer.Entities;

namespace Carola.DataAccessLayer.EntityFramework;

public class EfLocationDal:GenericRepository<Location>
{
    public EfLocationDal(CarolaContext context) : base(context)
    {
    }
}