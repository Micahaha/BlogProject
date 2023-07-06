using BlogProject.Data;
using BlogProject.Interfaces;
using BlogProject.Models;

namespace BlogProject.Repositories
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {

        public TagRepository(ApplicationDbContext context) : base(context)
        {
        }

        public List<Tag> GetAllTags()
        {
            return _context.Tags.ToList();  
        }
    }
}
