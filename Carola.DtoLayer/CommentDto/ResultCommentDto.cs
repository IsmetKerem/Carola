// ResultCommentDto
namespace Carola.DtoLayer.CommentDtos
{
    public class ResultCommentDto
    {
        public int CommentId { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public string CommentText { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CarId { get; set; }
    }
}

// CreateCommentDto
namespace Carola.DtoLayer.CommentDtos
{
    public class CreateCommentDto
    {
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public string CommentText { get; set; }
        public int Rating { get; set; }
        public int CarId { get; set; }
    }
}

// UpdateCommentDto
namespace Carola.DtoLayer.CommentDtos
{
    public class UpdateCommentDto
    {
        public int CommentId { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public string CommentText { get; set; }
        public int Rating { get; set; }
        public int CarId { get; set; }
    }
}

// GetByIdCommentDto
namespace Carola.DtoLayer.CommentDtos
{
    public class GetByIdCommentDto
    {
        public int CommentId { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public string CommentText { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CarId { get; set; }
    }
}