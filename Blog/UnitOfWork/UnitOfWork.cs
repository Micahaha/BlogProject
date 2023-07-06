using BlogProject.Data;
using BlogProject.Interfaces;
using BlogProject.Repositories;

namespace BlogProject.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context) 
        {
            _context = context;
            Authors = new AuthorRepository(_context);
            Blogs = new BlogRepository(_context);
            Tags = new TagRepository(_context);
        }

        public IAuthorRepository Authors { get; private set; }
        public IBlogRepository Blogs { get; private set; }
        public ITagRepository Tags { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
