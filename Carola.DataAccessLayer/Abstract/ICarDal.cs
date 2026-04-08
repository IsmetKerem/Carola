using Carola.EntityLayer.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carola.DataAccessLayer.Abstract
{
    public interface ICarDal:IGenericDal<Car>
    {
        Task<List<Car>> GetAllCarsWithCategoryAsync();
    }
}
