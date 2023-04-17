namespace Tigernet.Hosting.DataAccess.Models.QueryOptions;

/// <summary>
/// Defines properties for queryable source query options
/// </summary>
/// <typeparam name="TModel">Query source type</typeparam>
public interface IQueryOptions<TModel> where TModel : class
{
    /// <summary>
    /// Query searching options
    /// </summary>
    SearchOptions<TModel>? SearchOptions { get; set; }

    /// <summary>
    /// Applied filters for a query
    /// </summary>
    FilterOptions<TModel>? FilterOptions { get; set; }

    /// <summary>
    /// Result Sort options 
    /// </summary>
    SortOptions<TModel>? SortOptions { get; set; }

    /// <summary>
    /// Calculated pagination options
    /// </summary>
    PaginationOptions PaginationOptions { get; set; }
}