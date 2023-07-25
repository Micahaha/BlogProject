using BlogProject.Data;
using Microsoft.AspNetCore.Identity;

namespace BlogProject.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;
        
        public UserService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        { 
            this.httpContextAccessor = httpContextAccessor;
            this.context = context;
            this.userManager = userManager;
        }

    }
}
