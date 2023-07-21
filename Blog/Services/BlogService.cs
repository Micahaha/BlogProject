﻿
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


        public async Task<ApplicationUser> getUserAsync()
        {
            var userId = userManager.GetUserId(httpContextAccessor.HttpContext.User);
            var user = await userManager.Users
                .Include(u => u.LikedComments)
                .Include(d => d.DislikedComments)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }

        public void AddComment(int blogId, string text, string username) 
        {
            var comment = new Comment { Likes = 0, Dislikes = 0, Text = text, User = username};
            var current_blog = context.Blogs.Where(blog => blog.BlogId == blogId).Include(c => c.Comments).SingleOrDefault();
            current_blog.Comments.Add(comment);
            context.SaveChanges();
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



    }
}
