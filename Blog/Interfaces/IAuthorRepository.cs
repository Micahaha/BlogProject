using BlogProject.Models;

namespace BlogProject.Interfaces
{
    public interface IAuthorRepository : IGenericRepository<Author>
    {
        public List<Author> GetAllAuthors();
    }
}
