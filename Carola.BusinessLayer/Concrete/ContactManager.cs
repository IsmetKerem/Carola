using AutoMapper;
using Carola.BusinessLayer.Abstract;
using Carola.DataAccessLayer.Repositories.ContactRepository;
using Carola.DtoLayer.ContactDtos;
using Carola.EntityLayer.Entities;

namespace Carola.BusinessLayer.Concrete
{
    public class ContactManager : GenericManager<ResultContactDto, Contact>, IContactService
    {
        private readonly IContactDal _contactDal;
        private readonly IMapper _mapper;

        public ContactManager(IContactDal contactDal, IMapper mapper) : base(contactDal, mapper)
        {
            _contactDal = contactDal;
            _mapper = mapper;
        }

        public async Task<List<ResultContactDto>> TGetUnreadMessagesAsync()
        {
            var messages = await _contactDal.GetUnreadMessagesAsync();
            return _mapper.Map<List<ResultContactDto>>(messages);
        }

        public async Task<int> TGetUnreadCountAsync()
        {
            return await _contactDal.GetUnreadCountAsync();
        }

        public async Task TMarkAsReadAsync(int contactId)
        {
            await _contactDal.MarkAsReadAsync(contactId);
        }

        public async Task TCreateContactAsync(CreateContactDto dto)
        {
            var entity = _mapper.Map<Contact>(dto);
            entity.SendDate = DateTime.Now;
            entity.IsRead = false;
            await _contactDal.InsertAsync(entity);
        }

        public async Task TUpdateContactAsync(UpdateContactDto dto)
        {
            var entity = _mapper.Map<Contact>(dto);
            await _contactDal.UpdateAsync(entity);
        }

        public async Task<GetByIdContactDto?> TGetByIdDtoAsync(int id)
        {
            var entity = await _contactDal.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<GetByIdContactDto>(entity);
        }
    }
}