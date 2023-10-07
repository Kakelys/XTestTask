using System.Linq.Expressions;

namespace XTestTask.Data.Repository.Interfaces
{
    public interface IRepositoryBase<T> where T : class
    {
        IQueryable<T> FindAll(bool asTracking = false);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool asTracking = false);
        T Create(T entity);
        Task<T> CreateAsync(T entity);
        void Delete(T entity);
        void DeleteMany(IEnumerable<T> entities);   
    }
}