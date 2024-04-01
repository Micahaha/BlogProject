using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace BlogProject.Models
{
    public class Blog
    {


        [Key]
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Tag Tag { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public Author Author { get; set; } = new Author { User = "" };
        [NotMapped]
        public IFormFile? Thumbnail { get; set; } 

        public string ?ThumbnailPathUrl { get; set; }

    }

    
}
