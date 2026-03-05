using Carola.EntityLayer.Entities;

namespace Carola.BusinessLayer.Abstract;

public interface ICustomerService:IGenericService<Customer>
{
    public Task TInsertAsync(Customer entity)
    {
        throw new NotImplementedException();
    }

    public Task TUpdateAsync(Customer entity)
    {
        throw new NotImplementedException();
    }

    public Task TDeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Customer>> TGetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Customer> TGetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}