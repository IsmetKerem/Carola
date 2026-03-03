namespace Carola.DataAccessLayer.Abstract;

public interface IGenericDal<T>
{
    Task InsertAsync (T entity);
    Task UpdateAsync  (T entity);
    Task DeleteAsync  (T entity);
    Task <List<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    
}