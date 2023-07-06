using BlogProject.Data;
using BlogProject.Models;
using BlogProject.Repositories;

namespace BlogProject.Services
{
    public class BlogService
    {
        // Inject database dependency 
        private readonly BlogRepository _blogRepository;
        public BlogService(BlogRepository blogRepository) 
        {
            _blogRepository = blogRepository;
        }

        // Get each Blog and put it in a list
        public List<Blog> GetAllBlogs() 
        {
            return _blogRepository.GetAllBlogs();
        }
    }
}
