using BlogProject.Data;
using BlogProject.Models;
using BlogProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public IActionResult Like(int blogId, int commentId) 
        {
            blogService.AddLike(blogId, commentId);
            var user = blogService.getUserAsync();

            var current_blog = context.Blogs.Where(context => context.BlogId == blogId).FirstOrDefault();
            var current_comment = current_blog.Comments.Where(comment => comment.Id == commentId).FirstOrDefault();

            if (user.LikedComments.Contains(current_comment))
            {
                ViewBag.isLiked = true;
            }

            else 
            {
                ViewBag.Liked = false;
            }

            context.SaveChanges();
            return  RedirectToAction("Index");
        }
        public IActionResult Dislike(int blogId, int commentId)
        {
            blogService.AddDislike(blogId, commentId);
            context.SaveChanges();
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

            return RedirectToAction("Index");
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