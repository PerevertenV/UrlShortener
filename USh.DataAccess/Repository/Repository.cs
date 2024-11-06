using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using USh.DataAccess.Data;
using USh.DataAccess.Repository.IRepository;

namespace USh.DataAccess.Repository
{
    internal class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            this.dbSet =  context.Set<T>();
        }

        public void Add(T entity)
        {
             dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null) { query = query.Where(filter); }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query =  dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
            _context.SaveChangesAsync();
        }
    }
}