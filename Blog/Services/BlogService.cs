
using BlogProject.Data;
using BlogProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.Services
{
    public class BlogService
    {
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;


        public BlogService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) 
        {
            this.context = context;
            this.httpContextAccessor = httpContextAccessor;
        }
        // Get each Blog and put it in a list
        public List<Blog> GetAllBlogs() 
        {
            return context.Blogs.Include(b => b.Author)
                .Include(b => b.Comments)
                .Include(b => b.Tag)
                .ToList();   
        }

        public void AddComment(int blogId, string text, string username) 
        {
            var comment = new Comment { Likes = 0, Dislikes = 0, Text = text, User = username};
            var current_blog = context.Blogs.Where(blog => blog.BlogId == blogId).Include(c => c.Comments).SingleOrDefault();
            current_blog.Comments.Add(comment);
            context.SaveChanges();
        }

        public void AddLike(int blogId, int CommentId) 
        {
            var current_blog = context.Blogs.Where(blog => blog.BlogId == blogId).Include(c => c.Comments).SingleOrDefault();
            var current_comment = current_blog.Comments.Where(comment => comment.Id == CommentId).SingleOrDefault();
            current_comment.Likes++;
        }
        public void AddDislike(int blogId, int CommentId)
        {
            var current_blog = context.Blogs.Where(blog => blog.BlogId == blogId).Include(c => c.Comments).SingleOrDefault();
            var current_comment = current_blog.Comments.Where(comment => comment.Id == CommentId).SingleOrDefault();
            current_comment.Dislikes++;
        }



    }
}
