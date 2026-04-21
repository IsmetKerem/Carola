using Carola.DataAccessLayer.Repositories.GenericRepository;
using Carola.EntityLayer.Entities;

namespace Carola.DataAccessLayer.Repositories.CommentRepository
{
    public interface ICommentDal : IGenericDal<Comment>
    {
        Task<List<Comment>> GetCommentsByCarIdAsync(int carId);
    }
}

