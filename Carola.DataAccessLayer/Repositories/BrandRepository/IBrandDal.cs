using Carola.DataAccessLayer.Repositories.GenericRepository;
using Carola.EntityLayer.Entities;

namespace Carola.DataAccessLayer.Repositories.BrandRepository
{
    public interface IBrandDal : IGenericDal<Brand>
    {
        Task<List<Brand>> GetFeaturedBrandsAsync(); 
    }
}