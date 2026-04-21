using Carola.DtoLayer.ContactDtos;

namespace Carola.BusinessLayer.Abstract
{
    public interface IContactService : IGenericService<ResultContactDto>
    {
        Task<List<ResultContactDto>> TGetUnreadMessagesAsync();
        Task<int> TGetUnreadCountAsync();
        Task TMarkAsReadAsync(int contactId);
        Task TCreateContactAsync(CreateContactDto dto);
        Task TUpdateContactAsync(UpdateContactDto dto);
        Task<GetByIdContactDto?> TGetByIdDtoAsync(int id);
    }
}