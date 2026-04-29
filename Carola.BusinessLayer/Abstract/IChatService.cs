using Carola.DtoLayer.ChatDtos;

namespace Carola.BusinessLayer.Abstract
{
    public interface IChatService
    {
      
        Task<ChatResponseDto> RecommendCarsAsync(string userMessage);
    }
}