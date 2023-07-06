using BlogProject.Data;
using BlogProject.Interfaces;
using BlogProject.Models;

namespace BlogProject.Repositories
{
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {

        public BlogRepository(ApplicationDbContext context) : base(context)
        {
        }

        public List<Blog> GetAllBlogs()
        {
            return _context.Blogs.ToList();
        }
    }
}
