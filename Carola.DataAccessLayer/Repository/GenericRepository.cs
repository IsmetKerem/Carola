using Carola.DataAccessLayer.Abstract;
using Carola.DataAccessLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace Carola.DataAccessLayer.Repository;

public class GenericRepository<T> :IGenericDal<T> where T : class
{
    private readonly CarolaContext _context;
    public GenericRepository(CarolaContext context)
    {
        _context = context;
    }
    public async Task InsertAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var values = await _context.Set<T>().FindAsync(id);
        _context.Set<T>().Remove(values);
        await _context.SaveChangesAsync();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }
}