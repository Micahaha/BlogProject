using BlogProject.Models;

namespace BlogProject.Interfaces
{
    public interface ITagRepository : IGenericRepository<Tag>
    {
        public List<Tag> GetAllTags();
    }
}
