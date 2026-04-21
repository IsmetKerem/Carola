namespace Carola.BusinessLayer.Abstract
{
    public interface IGenericService<T> where T : class
    {
        Task<List<T>> TGetListAsync();
        Task<T?> TGetByIdAsync(int id);
        Task TInsertAsync(T entity);
        Task TUpdateAsync(T entity);
        Task TDeleteAsync(int id);
    }
}