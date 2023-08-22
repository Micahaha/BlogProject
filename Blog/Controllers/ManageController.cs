using BlogProject.Data;
using BlogProject.Models;
using BlogProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BlogProject.Controllers
{
    public class ManageController : Controller
    {

        private readonly BlogService blogService;
        private readonly ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        public ManageController(BlogService blogService, ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.blogService = blogService;
            this.context = context;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }


        public IActionResult Index()
        {
            return RedirectToAction(nameof(List));
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
        public async Task<IActionResult> Delete(string id) 
        {
            var selected_role = await roleManager.FindByIdAsync(id);
            if (selected_role != null)
                await roleManager.DeleteAsync(selected_role);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var users =  userManager.Users;

            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };


            // Retrieve all the Users
            foreach (var user in users)
            {
                // If the user is in this role, add the username to
                // Users property of EditRoleViewModel. This model
                // object is then passed to the view for display
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View(nameof(Error));
            }
            else
            {
                role.Name = model.RoleName;

                // Update the Role using UpdateAsync
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        public IActionResult List()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        public IActionResult Add() 
        {
            var users = userManager.Users;
            return View(users);
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(string id) 
        {

            var user =  await userManager.FindByIdAsync(id);
            var result = await userManager.AddToRoleAsync(user, "Admin");

            if (result.Succeeded)
                return RedirectToAction("Index", "home");

            TempData["ToastMessage"] = "An error occurred: " + result.Errors;

            var users = userManager.Users;
            
            return View(users);

        }

        public async Task<IActionResult> RemoveAsync(string roleId)
        {
            var current_role = await roleManager.FindByIdAsync(roleId);
            var role_name = await roleManager.GetRoleNameAsync(current_role);
            var users = userManager.Users;
            var roles = new List<IdentityRole>();

            foreach (var user in users) 
            {
                if (await userManager.IsInRoleAsync(user, role_name)) 
                {
                    roles.Add(await roleManager.FindByNameAsync(role_name));
                }
            }
            return View(users);
        }
        [HttpPost]
        public async Task<IActionResult> Remove(string id)
        {

            var user = await userManager.FindByIdAsync(id);
            var result = await userManager.RemoveFromRoleAsync(user, "Admin");


            if (result.Succeeded)
                return RedirectToAction("Index", "home");

            TempData["ToastMessage"] = "An error occurred: " + result.Errors;

            var users = userManager.Users;

            return View(users);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
