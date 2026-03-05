namespace Carola.BusinessLayer.Abstract;

public interface IGenericService<T> where T: class
{
    Task TInsertAsync (T entity);
    Task TUpdateAsync  (T entity);
    Task TDeleteAsync  (int id);
    Task <List<T>> TGetAllAsync();
    Task<T> TGetByIdAsync(int id);
}