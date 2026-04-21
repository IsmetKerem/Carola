using Carola.DataAccessLayer.Context;
using Carola.DataAccessLayer.Repositories.GenericRepository;
using Carola.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carola.DataAccessLayer.Repositories.CommentRepository
{
    public class EfCommentDal : GenericRepository<Comment>, ICommentDal
    {
        private readonly CarolaContext _context;
        public EfCommentDal(CarolaContext context) : base(context) { _context = context; }

        public async Task<List<Comment>> GetCommentsByCarIdAsync(int carId)
        {
            return await _context.Comments.AsNoTracking()
                .Where(c => c.CarId == carId)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }
    }
}