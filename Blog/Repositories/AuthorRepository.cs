using BlogProject.Data;
using BlogProject.Interfaces;
using BlogProject.Models;

namespace BlogProject.Repositories
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {

        public AuthorRepository(ApplicationDbContext context) : base(context)
        {
        }

        public List<Author> GetAllAuthors()
        {
           return _context.Authors.ToList();
        }

    }
}
