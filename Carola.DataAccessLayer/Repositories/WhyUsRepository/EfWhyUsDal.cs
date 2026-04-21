using Carola.DataAccessLayer.Context;
using Carola.DataAccessLayer.Repositories.GenericRepository;
using Carola.EntityLayer.Entities;

namespace Carola.DataAccessLayer.Repositories.WhyUsRepository
{
    public class EfWhyUsDal : GenericRepository<WhyUs>, IWhyUsDal
    {
        public EfWhyUsDal(CarolaContext context) : base(context) { }
    }
}