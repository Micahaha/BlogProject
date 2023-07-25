using BlogProject.Data;
using BlogProject.Models;
using BlogProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ManageController : Controller
    {

        private readonly BlogService blogService;
        private readonly ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        public ManageController(BlogService blogService, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            this.blogService = blogService;
            this.context = context;
            this.roleManager = roleManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create() 
        {
            return View();
        }

        public IActionResult Roles() 
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Role role) 
        {
            if (ModelState.IsValid) 
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = role.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded) 
                {
                    return RedirectToAction("Index", "home");
                }

                foreach (IdentityError error in result.Errors) 
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(role);
        }


    }
}
