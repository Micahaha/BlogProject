using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models
{
    public class Reply
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set;  }
        public string User { get; set; }
        public int? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;


    }
}