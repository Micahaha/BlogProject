
using BlogProject.Data;
using BlogProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

            return context.Blogs
                .Include(b => b.Author)
                .Include(b => b.Comments)
                .Include(b => b.Tag)
                .ToList();



        }

        public Blog GetBlog(int id)
        {
            var blog = context.Blogs
                .Where(blog => blog.BlogId == id)
                .Include(c => c.Comments).ThenInclude(r => r.Replies)
                .Include(t => t.Tag)
                .Include(a => a.Author)
                .FirstOrDefault();

            if (blog != null)
            {
                return blog;
            }

            return null;
        }


        public async Task<ApplicationUser> getUserAsync()
        {
            var userId = userManager.GetUserId(httpContextAccessor.HttpContext.User);
            var user = await userManager.Users
                .Include(u => u.LikedComments)
                .Include(d => d.DislikedComments)
                .Include(c => c.LikedReplies)
                .Include(l => l.DislikedReplies)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }

        public void AddComment(int blogId, string text, string username)
        {
            var comment = new Comment { Likes = 0, Dislikes = 0, Text = text, User = username, BlogId = blogId, Replies = new List<Reply>() };
            var current_blog = context.Blogs.Where(blog => blog.BlogId == blogId).Include(c => c.Comments).SingleOrDefault();
            current_blog.Comments.Add(comment);
            context.SaveChanges();
        }

        public void AddReply(int commentId, string text, string username)
        {
            // query to get current blog
            var current_blog = context.Comment
                .Where(comment => comment.Id == commentId)
                .Join(context.Blogs, comment => comment.BlogId, blog => blog.BlogId, (comment, blog) => blog).SingleOrDefault();

            // query to get parent comment
            var parent_comment = context.Comment
            .Where(comment => comment.Id == commentId).Include(r => r.Replies).SingleOrDefault();

            if (current_blog != null)
            {
                if (parent_comment.Replies == null)
                {
                    parent_comment.Replies = new List<Reply>();
                }
                var reply = new Reply { Likes = 0, Dislikes = 0, Text = text, User = username, ParentComment = parent_comment, ParentCommentId = parent_comment.Id };
                parent_comment.Replies.Add(reply);
            }

            context.SaveChanges();
        }

        public Blog getBlogFromComment(int commentId)
        {
            var blog_obj = context.Comment
                .Where(comment => comment.Id == commentId)
                .Join(context.Blogs, comment => comment.BlogId, blog => blog.BlogId, (comment, blog) => blog).SingleOrDefault();

            return blog_obj;
        }

        public async Task AddLike(int blogId, int commentId)
        {
            var user = await getUserAsync();
            if (user == null)
                return;


            var current_blog = await context.Blogs
                .Where(blog => blog.BlogId == blogId)
                .Include(c => c.Comments)
                .SingleOrDefaultAsync();

            if (current_blog == null)
                return;

            var current_comment = current_blog.Comments.FirstOrDefault(comment => comment.Id == commentId);

            if (current_comment == null)
                return;

            if (!user.LikedComments.Contains(current_comment))
            {
                user.LikedComments.Add(current_comment);

                if (user.DislikedComments.Contains(current_comment))
                {
                    user.DislikedComments.Remove(current_comment);

                    if (current_comment.Dislikes > 0)
                        current_comment.Dislikes--;
                }

                current_comment.Likes++;
            }
        }



        public int Check_Likes(int blogId, int commentId)
        {

            var current_blog = context.Blogs
                .Where(blog => blog.BlogId == blogId)
                .Include(c => c.Comments)
                .SingleOrDefault();

            if (current_blog == null)
                return -1;

            var current_comment = current_blog.Comments.FirstOrDefault(comment => comment.Id == commentId);

            if (current_comment == null)
                return -1;

            return current_comment.Likes;

        }

        public int Check_dislikes(int blogId, int commentId)
        {

            var current_blog = context.Blogs
                .Where(blog => blog.BlogId == blogId)
                .Include(c => c.Comments)
                .SingleOrDefault();

            if (current_blog == null)
                return -1;

            var current_comment = current_blog.Comments.FirstOrDefault(comment => comment.Id == commentId);

            if (current_comment == null)
                return -1;

            return current_comment.Dislikes;

        }

        public async Task AddDislike(int blogId, int commentId)
        {
            var user = await getUserAsync();

            if (user == null)
                return;

            var current_blog = await context.Blogs
                .Where(blog => blog.BlogId == blogId)
                .Include(c => c.Comments)
                .SingleOrDefaultAsync();

            if (current_blog == null)
                return;

            var current_comment = current_blog.Comments.FirstOrDefault(comment => comment.Id == commentId);

            if (current_comment == null)
                return;

            if (!user.DislikedComments.Contains(current_comment))
            {
                user.DislikedComments.Add(current_comment);

                if (user.LikedComments.Contains(current_comment))
                {
                    user.LikedComments.Remove(current_comment);

                    if (current_comment.Likes > 0)
                        current_comment.Likes--;
                }

                current_comment.Dislikes++;
            }
        }




        // Reply duplicate functions

        public async Task AddLike(int blogId, int commentId, int replyId)
        {
            var user = await getUserAsync();
            if (user == null)
                return;


            var current_blog = await context.Blogs
                .Where(blog => blog.BlogId == blogId)
                .Include(c => c.Comments)
                .ThenInclude(r => r.Replies)
                .SingleOrDefaultAsync();

            if (current_blog == null)
                return;

            var current_comment = current_blog.Comments.FirstOrDefault(comment => comment.Id == commentId);
            var current_reply = current_comment.Replies.FirstOrDefault(reply => reply.Id == replyId);

            if (current_comment == null)
                return;

            if (!user.LikedReplies.Contains(current_reply))
            {
                user.LikedReplies.Add(current_reply);

                if (user.DislikedReplies.Contains(current_reply))
                {
                    user.DislikedReplies.Remove(current_reply);

                    if (current_reply.Dislikes > 0)
                        current_reply.Dislikes--;
                }

                current_reply.Likes++;
            }
        }



        public int Check_Likes(int blogId, int commentId, int replyId)
        {

            var current_blog = context.Blogs
                .Where(blog => blog.BlogId == blogId)
                .Include(c => c.Comments)
                .ThenInclude(r => r.Replies)
                .SingleOrDefault();

            if (current_blog == null)
                return -1;

            var current_comment = current_blog.Comments.FirstOrDefault(comment => comment.Id == commentId);
            var current_reply = current_comment.Replies.FirstOrDefault(reply => reply.Id == replyId);

            if (current_reply == null)
                return -1;

            return current_reply.Likes;

        }

        public int Check_dislikes(int blogId, int commentId, int replyId)
        {

            var current_blog = context.Blogs
                .Where(blog => blog.BlogId == blogId)
                .Include(c => c.Comments)
                .ThenInclude(r => r.Replies)
                .SingleOrDefault();

            if (current_blog == null)
                return -1;

            var current_comment = current_blog.Comments.FirstOrDefault(comment => comment.Id == commentId);
            var current_reply = current_comment.Replies.FirstOrDefault(reply => reply.Id == replyId);

            if (current_comment == null)
                return -1;

            return current_reply.Dislikes;

        }

        public async Task AddDislike(int blogId, int commentId, int replyId)
        {
            var user = await getUserAsync();

            if (user == null)
                return;

            var current_blog = await context.Blogs
                .Where(blog => blog.BlogId == blogId)
                .Include(c => c.Comments)
                .ThenInclude(r => r.Replies)
                .SingleOrDefaultAsync();

            if (current_blog == null)
                return;

            var current_comment = current_blog.Comments.FirstOrDefault(comment => comment.Id == commentId);
            var current_reply = current_comment.Replies.FirstOrDefault(reply => reply.Id == replyId);


            if (current_comment == null)
                return;

            if (!user.DislikedReplies.Contains(current_reply))
            {
                user.DislikedReplies.Add(current_reply);

                if (user.LikedReplies.Contains(current_reply))
                {
                    user.LikedReplies.Remove(current_reply);

                    if (current_reply.Likes > 0)
                        current_reply.Likes--;
                }

                current_reply.Dislikes++;
            }
        }



    }
}
