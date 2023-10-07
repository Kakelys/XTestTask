using Microsoft.EntityFrameworkCore;
using XTestTask.DTO;

namespace XTestTask.Data.Repository.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> EnableTracking<T>(this IQueryable<T> query, bool enable) where T: class =>
            enable ? query.AsTracking() : query.AsNoTracking();

        public static IQueryable<T> TakePage<T>(this IQueryable<T> query, Page page) where T: class =>
            query.Skip(page.Offset).Take(page.Limit);
    }
}