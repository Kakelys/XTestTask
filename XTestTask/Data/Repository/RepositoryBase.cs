using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using XTestTask.Data.Repository.Extensions;
using XTestTask.Data.Repository.Interfaces;

namespace XTestTask.Data.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T: class
    {
        protected readonly AppDbContext _context;

        public RepositoryBase(AppDbContext context)
        {
            _context = context;
        }

        public T Create(T entity)
        {
            _context.Add(entity);

            return entity;
        }

        public async Task<T> CreateAsync(T entity) 
        {
            await _context.AddAsync(entity);

            return entity;
        }

        public void Delete(T entity) =>
            _context.Remove(entity);
        

        public void DeleteMany(IEnumerable<T> entities) =>
            _context.RemoveRange(entities);

        public IQueryable<T> FindAll(bool asTracking = false) =>
            _context.Set<T>().EnableTracking(asTracking);

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool asTracking = false) => 
            _context.Set<T>().EnableTracking(asTracking).Where(expression);
    }
}