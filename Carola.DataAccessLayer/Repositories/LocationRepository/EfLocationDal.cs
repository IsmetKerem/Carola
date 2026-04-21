using Carola.DataAccessLayer.Context;
using Carola.DataAccessLayer.Repositories.GenericRepository;
using Carola.EntityLayer.Entities;

namespace Carola.DataAccessLayer.Repositories.LocationRepository
{
    public class EfLocationDal : GenericRepository<Location>, ILocationDal
    {
        public EfLocationDal(CarolaContext context) : base(context) { }
    }
}