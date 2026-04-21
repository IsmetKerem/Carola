using System.Linq.Expressions;

namespace Carola.DataAccessLayer.Repositories.GenericRepository
{
    public interface IGenericDal<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        // Filtreleme için
        Task<List<T>> GetByFilterAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetSingleByFilterAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);

        // Include ile navigation property getirme (örn: Car getirirken Brand'i de çek)
        Task<List<T>> GetAllWithIncludeAsync(params Expression<Func<T, object>>[] includes);
    }
}