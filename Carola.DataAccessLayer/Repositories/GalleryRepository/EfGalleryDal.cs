using Carola.DataAccessLayer.Context;
using Carola.DataAccessLayer.Repositories.GenericRepository;
using Carola.EntityLayer.Entities;

namespace Carola.DataAccessLayer.Repositories.GalleryRepository
{
    public class EfGalleryDal : GenericRepository<Gallery>, IGalleryDal
    {
        public EfGalleryDal(CarolaContext context) : base(context) { }
    }
}