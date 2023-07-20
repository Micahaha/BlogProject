
using BlogProject.Data;
using BlogProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.Services
{
    public class BlogService
    {
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;

        public BlogService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager) 
        {
            this.context = context;
            this.userManager = userManager;
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


        public ApplicationUser getUserAsync() 
        {
            var userId = userManager.GetUserId(httpContextAccessor.HttpContext.User);
            var user = userManager.Users
                .Include(u => u.LikedComments)
                .Include(d => d.DislikedComments)
                .FirstOrDefault(u => u.Id == userId);

            return user;
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
            var user = getUserAsync();
            var current_blog = context.Blogs.Where(blog => blog.BlogId == blogId).Include(c => c.Comments).SingleOrDefault();
            var current_comment = current_blog.Comments.Where(comment => comment.Id == CommentId).SingleOrDefault();
            if (user != null) 
            {
                if (!(user.LikedComments.Contains(current_comment)))
                {

                    {
                        user.LikedComments.Add(current_comment);

                        if (user.DislikedComments.Contains(current_comment))
                        {
                            user.DislikedComments.Remove(current_comment);
                            current_comment.Dislikes--;
                        }

                        current_comment.Likes++;
                    }
                }
                else 
                {
                    return;
                }
            }

        }
        public void AddDislike(int blogId, int CommentId)
        {
            var current_blog = context.Blogs.Where(blog => blog.BlogId == blogId).Include(c => c.Comments).SingleOrDefault();
            var current_comment = current_blog.Comments.Where(comment => comment.Id == CommentId).SingleOrDefault();
            var user = getUserAsync();

            if (!(user.DislikedComments.Contains(current_comment)))
            {
                if (user != null)
                {
                    user.DislikedComments.Add(current_comment);
                    if (user.LikedComments.Contains(current_comment))
                    {
                        user.LikedComments.Remove(current_comment);
                        current_comment.Likes--;
                    }

                    current_comment.Dislikes++;
                }
            }
            else 
            {
                return;
            }

        }



    }
}
