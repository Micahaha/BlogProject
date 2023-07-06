using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }
        public string Name { get; set;  }
    }
}
