using Carola.DataAccessLayer.Repositories.GenericRepository;
using Carola.EntityLayer.Entities;

namespace Carola.DataAccessLayer.Repositories.ContactRepository
{
    public interface IContactDal : IGenericDal<Contact>
    {
        Task<List<Contact>> GetUnreadMessagesAsync();
        Task<int> GetUnreadCountAsync();
        Task MarkAsReadAsync(int contactId);
    }
}