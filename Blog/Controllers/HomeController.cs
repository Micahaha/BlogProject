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


            return View(blogService.GetAllBlogs());
           

        }

        [HttpPost]
        public IActionResult AddComment(int blogId, string text) 
        {
            if (ModelState.IsValid) 
            {
                blogService.AddComment(blogId, text, User.Identity.Name);
            }
            return RedirectToAction(nameof(Blog), new { id = blogId});
        }


        [HttpPost]
        public IActionResult _Reply(int commentId, string text)
        {
            if (ModelState.IsValid)
            {
                blogService.AddReply(commentId, text, User.Identity.Name);
            }

            return RedirectToAction(nameof(Blog), new { id = blogService.getBlogFromComment(commentId).BlogId});
        }

        public IActionResult _Reply()
        {
            return PartialView(nameof(_Reply));
        }



        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Like(int blogId, int commentId)
        {
            var updated_likes = 0;
            var updated_dislikes = 0;

            if (User.Identity.IsAuthenticated)
            {

                await blogService.AddLike(blogId, commentId);
                await context.SaveChangesAsync();
                updated_dislikes = blogService.Check_dislikes(blogId, commentId);
                updated_likes = blogService.Check_Likes(blogId, commentId);

                // UPDATE LIKES
            }

            var result = new { Dislikes = updated_dislikes, Likes = updated_likes };

            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Dislike(int blogId, int commentId)
        {
            var updated_dislikes = 0;
            var updated_likes = 0;

            if (User.Identity.IsAuthenticated)
            {

                await blogService.AddDislike(blogId, commentId);
                await context.SaveChangesAsync();
                updated_dislikes = blogService.Check_dislikes(blogId, commentId);
                updated_likes = blogService.Check_Likes(blogId, commentId);

                // UPDATE LIKES
            }
            var result = new { Dislikes = updated_dislikes, Likes = updated_likes };

            return Json(result);
        }


        public IActionResult Like() 
        {
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Dislike() 
        {
            return RedirectToAction(nameof(Index));
        }

       

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