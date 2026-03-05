using Carola.DataAccessLayer.Concrete;
using Carola.DataAccessLayer.Repository;
using Carola.EntityLayer.Entities;

namespace Carola.DataAccessLayer.EntityFramework;

public class EfCarDal:GenericRepository<Car>
{
    public EfCarDal(CarolaContext context) : base(context)
    {
    }
}