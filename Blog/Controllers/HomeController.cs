using BlogProject.Data;
using BlogProject.Models;
using BlogProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; 
        private readonly BlogService _blogService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, BlogService blogService)
        {
            _logger = logger;
            _context = context;
            _blogService = blogService;
        }

        public IActionResult Index()
        {
            List<Models.Blog> blogs = _blogService.GetAllBlogs();
            return View(blogs);
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