using Carola.DataAccessLayer.Concrete;
using Carola.DataAccessLayer.Repository;
using Carola.EntityLayer.Entities;

namespace Carola.DataAccessLayer.EntityFramework;

public class EfCustomerDal:GenericRepository<Customer>
{
    public EfCustomerDal(CarolaContext context) : base(context)
    {
    }
}