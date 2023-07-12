using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set;  }
        public string User { get; set; }


    }
}