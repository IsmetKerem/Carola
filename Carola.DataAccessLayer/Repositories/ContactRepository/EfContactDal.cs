using Carola.DataAccessLayer.Context;
using Carola.DataAccessLayer.Repositories.GenericRepository;
using Carola.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Carola.DataAccessLayer.Repositories.ContactRepository
{
    public class EfContactDal : GenericRepository<Contact>, IContactDal
    {
        private readonly CarolaContext _context;

        public EfContactDal(CarolaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Contact>> GetUnreadMessagesAsync()
        {
            return await _context.Contacts.AsNoTracking()
                .Where(c => !c.IsRead)
                .OrderByDescending(c => c.SendDate)
                .ToListAsync();
        }

        public async Task<int> GetUnreadCountAsync()
        {
            return await _context.Contacts.CountAsync(c => !c.IsRead);
        }

        public async Task MarkAsReadAsync(int contactId)
        {
            var contact = await _context.Contacts.FindAsync(contactId);
            if (contact != null)
            {
                contact.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}