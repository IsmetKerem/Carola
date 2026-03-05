using Carola.DataAccessLayer.Concrete;
using Carola.DataAccessLayer.Repository;
using Carola.EntityLayer.Entities;

namespace Carola.DataAccessLayer.EntityFramework;

public class EfReservationDal:GenericRepository<Reservation>
{
    public EfReservationDal(CarolaContext context) : base(context)
    {
    }
}