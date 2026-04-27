using Carola.DtoLayer.MailDtos;

namespace Carola.BusinessLayer.Abstract
{
    public interface IMailService
    {

        Task<bool> SendAsync(MailMessageDto message);
    }
}