using Carola.DtoLayer.CommentDtos;

namespace Carola.BusinessLayer.Abstract
{
    public interface ICommentService : IGenericService<ResultCommentDto>
    {
        Task<List<ResultCommentDto>> TGetCommentsByCarIdAsync(int carId);
        Task TCreateCommentAsync(CreateCommentDto dto);
        Task TUpdateCommentAsync(UpdateCommentDto dto);
        Task<GetByIdCommentDto?> TGetByIdDtoAsync(int id);
    }
}