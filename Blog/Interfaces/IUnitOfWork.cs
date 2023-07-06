namespace BlogProject.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthorRepository Authors { get; }
        IBlogRepository Blogs { get; }
        ITagRepository Tags { get; }
        int Complete();


    }
}
