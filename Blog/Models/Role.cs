using System.ComponentModel.DataAnnotations;

namespace BlogProject.Models
{
    public class Role
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
