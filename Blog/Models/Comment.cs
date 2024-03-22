using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string User { get; set; }
        public string Text { get; set;  }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        public int Likes { get; set; }
        public int Dislikes { get; set; }
           
        public int BlogId { get; set; }
        public List<Comment> Replies { get; set; }
    }
}