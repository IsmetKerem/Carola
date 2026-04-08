using Carola.DataAccessLayer.Abstract;
using Carola.DataAccessLayer.Concrete;
using Carola.DataAccessLayer.Repository;
using Carola.EntityLayer.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.DataAccessLayer.EntityFramework
{
    public class EfCarDal : GenericRepository<Car>, ICarDal
    {
        public EfCarDal(CarolaContext context) : base(context)
        {
        }

        public async Task<List<Car>> GetAllCarsWithCategoryAsync()
        {
            var context = new CarolaContext();
            var values = await context.Cars.Include(x => x.Category).ToListAsync();
            return values;
        }
    }
}