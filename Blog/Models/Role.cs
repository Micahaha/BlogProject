using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models
{
    public class Role
    {
        [Required]
        public string RoleName { get; set; }
    }
}
