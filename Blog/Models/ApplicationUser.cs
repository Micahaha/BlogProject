using Microsoft.AspNetCore.Identity;

namespace BlogProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Comment> LikedComments { get; set; }
        public List<Comment> DislikedComments { get; set;  }
        
        public List<Reply> LikedReplies { get; set; }
        public List<Reply> DislikedReplies { get; set; }
        
    }
}
