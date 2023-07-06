using BlogProject.Models;

namespace BlogProject.Interfaces
{
    public interface IBlogRepository : IGenericRepository<Blog>
    {
        public List<Blog> GetAllBlogs();
    }
}
