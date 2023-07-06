using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models
{
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Tag Tag { get; set;  }
        public Author Author { get; set; }

    }
}
