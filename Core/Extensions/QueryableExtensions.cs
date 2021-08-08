using System.Linq;

namespace Core.Extensions
{
    /// <summary>
    /// Расширения для linq-запросов в бд 
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Пагинация
        /// </summary>
        public static IQueryable<TSource> Paginate<TSource>(this IQueryable<TSource> query, int limit, int page)
        {
            return query.Skip((page - 1) * limit).Take(limit);
        }
    }
}