using AutoMapper;
using Carola.BusinessLayer.Abstract;
using Carola.DataAccessLayer.Repositories.CommentRepository;
using Carola.DtoLayer.CommentDtos;
using Carola.EntityLayer.Entities;

namespace Carola.BusinessLayer.Concrete
{
    public class CommentManager : GenericManager<ResultCommentDto, Comment>, ICommentService
    {
        private readonly ICommentDal _commentDal;
        private readonly IMapper _mapper;

        public CommentManager(ICommentDal commentDal, IMapper mapper) : base(commentDal, mapper)
        {
            _commentDal = commentDal;
            _mapper = mapper;
        }

        public async Task<List<ResultCommentDto>> TGetCommentsByCarIdAsync(int carId)
        {
            var comments = await _commentDal.GetCommentsByCarIdAsync(carId);
            return _mapper.Map<List<ResultCommentDto>>(comments);
        }

        public async Task TCreateCommentAsync(CreateCommentDto dto)
        {
            var entity = _mapper.Map<Comment>(dto);
            entity.CreatedDate = DateTime.Now;
            await _commentDal.InsertAsync(entity);
        }

        public async Task TUpdateCommentAsync(UpdateCommentDto dto)
        {
            var entity = _mapper.Map<Comment>(dto);
            await _commentDal.UpdateAsync(entity);
        }

        public async Task<GetByIdCommentDto?> TGetByIdDtoAsync(int id)
        {
            var entity = await _commentDal.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<GetByIdCommentDto>(entity);
        }
    }
}