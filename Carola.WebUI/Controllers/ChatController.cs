using Carola.BusinessLayer.Abstract;
using Carola.DtoLayer.ChatDtos;
using Microsoft.AspNetCore.Mvc;

namespace Carola.WebUI.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> Recommend([FromBody] ChatRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Message))
            {
                return Json(new ChatResponseDto
                {
                    Success = false,
                    ErrorMessage = "Mesaj boş olamaz."
                });
            }

            if (request.Message.Length > 500)
            {
                return Json(new ChatResponseDto
                {
                    Success = false,
                    ErrorMessage = "Mesaj çok uzun, lütfen kısaltın (max 500 karakter)."
                });
            }

            var result = await _chatService.RecommendCarsAsync(request.Message);
            return Json(result);
        }
    }
}