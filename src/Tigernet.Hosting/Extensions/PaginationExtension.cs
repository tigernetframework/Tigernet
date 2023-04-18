using System.IO;
using Tigernet.Hosting.Models.Query;

namespace Tigernet.Hosting.Extensions
{
    public static class PaginationExtension
    {
        /// <summary>
        /// Paginates an IEnumerable<T> object.
        /// </summary>
        /// <typeparam name="T">The type of object in the IEnumerable<T>.</typeparam>
        /// <param name="source">The IEnumerable<T> object to paginate.</param>
        /// <param name="options">The pagination options.</param>
        /// <returns>An IEnumerable<T> object that contains the items for the current page.</returns>
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source, PaginationOptions options)
        {
            if (source == null || options == null)
                throw new ArgumentException("Can't apply filter to null source or with null filter options");

            var itemsForSkip = (options.PageToken - 1) * options.PageSize;
            var totalCount = source.Count();
            if (itemsForSkip >= totalCount && totalCount > 0)
                itemsForSkip = totalCount - totalCount % options.PageSize;
            return source.Skip(itemsForSkip).Take(options.PageSize);
        }
        /// <summary>
        /// Paginates an IQueryable<T> object.
        /// </summary>
        /// <typeparam name="T">The type of object in the IQueryable<T>.</typeparam>
        /// <param name="source">The IQueryable<T> object to paginate.</param>
        /// <param name="options">The pagination options.</param>
        /// <returns>An IQueryable<T> object that contains the items for the current page.</returns>
        public static IQueryable<T> Paginate<T>(this IQueryable<T> source, PaginationOptions options)
        {
            if (source == null || options == null)
                throw new ArgumentException("Can't apply filter to null source or with null filter options");
            
            var itemsForSkip = (options.PageToken - 1) * options.PageSize;
            var totalCount = source.Count();
            if (itemsForSkip >= totalCount && totalCount > 0)
                itemsForSkip = totalCount - totalCount % options.PageSize;
            return source.Skip(itemsForSkip).Take(options.PageSize);
        }

    }
}
