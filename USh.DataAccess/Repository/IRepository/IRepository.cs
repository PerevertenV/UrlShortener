using System.Linq.Expressions;

namespace USh.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null);
        T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}