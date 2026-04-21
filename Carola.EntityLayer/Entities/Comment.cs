using System;

namespace Carola.EntityLayer.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string UserName { get; set; }     
        public string UserImage { get; set; }    
        public string CommentText { get; set; }
        public int Rating { get; set; }           
        public DateTime CreatedDate { get; set; }

        // Foreign Key + Navigation
        public int CarId { get; set; }
        public Car Car { get; set; }
    }
}