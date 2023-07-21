using BlogProject.Data;
using BlogProject.Models;
using BlogProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BlogProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlogService blogService;
        private readonly ApplicationDbContext context;
        public HomeController(BlogService blogService, ApplicationDbContext context)
        {
            this.blogService = blogService;
            this.context = context;
        }

        public IActionResult Index()
        {
            var blogs = blogService.GetAllBlogs();
            return View(blogs);
        }

        [HttpPost]
        public IActionResult AddComment(int blogId, string text) 
        {
            if (ModelState.IsValid) 
            {
                blogService.AddComment(blogId, text, User.Identity.Name);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Like(int blogId, int commentId)
        {
            if (User.Identity.IsAuthenticated)
            {
                await blogService.AddLike(blogId, commentId);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Blog));

            }
            else
            {
                return View("Index");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Dislike(int blogId, int commentId)
        {
            if (User.Identity.IsAuthenticated)
            {
                await blogService.AddDislike(blogId, commentId);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");

            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Post()
        {
            List<SelectListItem> options = new List<SelectListItem>()
            {
                new SelectListItem {Value = "Fun", Text = "Fun" },
                new SelectListItem {Value = "Coding", Text = "Coding" },
                new SelectListItem {Value = "Miscellaneous", Text = "Miscellaneous" },
                new SelectListItem {Value = "Artwork", Text = "Artwork" },
            };

            ViewData["Options"] = options;

            return View();
        }


        [HttpPost]
        [Authorize]
        public IActionResult Post(Blog blog) 
        { 
            blog.Comments = new List<Comment>();
            blog.Author = new Author { User = User.Identity.Name };
           
            context.Add(blog);
            context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Blog(int id) 
        {
           var blog = blogService.GetBlog(id);
            return View(blog);
        }
       
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}