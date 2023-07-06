using System.Linq.Expressions;

namespace BlogProject.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        // Takes the type of class using the interface and returns the ID (in db in this example) 
        T GetById(int id);
        IEnumerable<T> GetAll();

        // Takes an argument of type T and returns a bool value (done this way because it's an interface) 
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);

        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
