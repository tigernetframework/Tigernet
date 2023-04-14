using System.Linq.Expressions;
using Tigernet.Hosting.DataAccess.Models.Entity;
using Tigernet.Hosting.DataAccess.Models.QueryOptions;

namespace Tigernet.Hosting.DataAccess.Models.Query;

/// <summary>
/// Provides extension methods to create query options
/// </summary>
public static class QueryGeneratorExtensions
{
    #region Generating

    /// <summary>
    /// Generates query for a type
    /// </summary>
    /// <param name="sourceType">Query source type</param>
    /// <typeparam name="TModel">Query source type</typeparam>
    /// <returns>Created query options</returns>
    public static IQueryOptions<TModel> CreateQuery<TModel>(this Type sourceType) where TModel : class
    {
        return sourceType != null ? new QueryOptions<TModel>() : throw new ArgumentException("Can't create query options for null source");
    }

    /// <summary>
    /// Generates query for a entity type
    /// </summary>
    /// <param name="sourceType">Query source type</param>
    /// <typeparam name="TEntity">Query source type</typeparam>
    /// <returns>Created query options</returns>
    public static IEntityQueryOptions<TEntity> CreateQuery<TEntity>(this TEntity sourceType) where TEntity : class, IQueryableEntity
    {
        return sourceType != null
            ? new EntityQueryOptions<TEntity>()
            : throw new ArgumentException("Can't create entity query options for null entity source");
    }

    #endregion

    #region Adding search

    /// <summary>
    /// Adds search options to given query options
    /// </summary>
    /// <param name="options">The query options</param>
    /// <param name="keyword">Search keyword</param>
    /// <param name="includeChildren">Determines whether to include children</param>
    /// <typeparam name="TModel">Query source type</typeparam>
    /// <returns>Updated query options</returns>
    /// <exception cref="ArgumentNullException">If given options is null</exception>
    public static IQueryOptions<TModel> AddSearch<TModel>(this IQueryOptions<TModel> options, string keyword, bool includeChildren = false)
        where TModel : class
    {
        if (options == null || string.IsNullOrWhiteSpace(keyword))
            throw new ArgumentException("Can't add search options to null query options or with empty keyword");

        options.SearchOptions = new SearchOptions<TModel>(keyword, includeChildren);
        return options;
    }

    /// <summary>
    /// Adds search options to given query options
    /// </summary>
    /// <param name="options">The query options</param>
    /// <param name="keyword">Search keyword</param>
    /// <param name="includeChildren">Determines whether to include children</param>
    /// <typeparam name="TEntity">Query source type</typeparam>
    /// <returns>Updated query options</returns>
    /// <exception cref="ArgumentNullException">If given options is null</exception>
    public static IEntityQueryOptions<TEntity> AddSearch<TEntity>(
        this IEntityQueryOptions<TEntity> options,
        string keyword,
        bool includeChildren = false
    ) where TEntity : class, IQueryableEntity
    {
        if (options == null || string.IsNullOrWhiteSpace(keyword))
            throw new ArgumentException("Can't add search options to null entity query options or with empty keyword");

        options.SearchOptions = new SearchOptions<TEntity>(keyword, includeChildren);
        return options;
    }

    #endregion

    #region Adding filter

    /// <summary>
    /// Adds search options to given query options
    /// </summary>
    /// <param name="options">The query options</param>
    /// <param name="keySelector">Model filter property selector</param>
    /// <param name="value">Value to filter with</param>
    /// <typeparam name="TModel">Query source type</typeparam>
    /// <typeparam name="TOptions">Query options</typeparam>
    ///  <returns>Updated query options</returns>
    /// <exception cref="ArgumentNullException">If query options, key selector or value is null</exception>
    public static TOptions AddFilter<TModel, TOptions>(this TOptions options, Expression<Func<TModel, object>> keySelector, object value)
        where TModel : class where TOptions : IQueryOptions<TModel>
    {
        if (options == null || keySelector == null)
            throw new ArgumentException("Can't add filter to null query options or with null key selector");

        // Get property name
        var memberExpression = keySelector.GetMemberExpression();
        var propertyName = memberExpression?.Member.Name ?? throw new InvalidOperationException();

        // TODO : Check value to property type
        options.FilterOptions = options.FilterOptions ?? new FilterOptions<TModel>();
        options.FilterOptions.Filters.Add(new QueryFilter(propertyName, value.ToString()));

        return options;
    }

    /// <summary>
    /// Adds search options to given query options
    /// </summary>
    /// <param name="options">The query options</param>
    /// <param name="keySelector">Model filter property selector</param>
    /// <param name="value">Value to filter with</param>
    /// <typeparam name="TEntity">Query source type</typeparam>
    ///  <returns>Updated query options</returns>
    /// <exception cref="ArgumentNullException">If query options, key selector or value is null</exception>
    public static IEntityQueryOptions<TEntity> AddFilter<TEntity>(
        this IEntityQueryOptions<TEntity> options,
        Expression<Func<TEntity, object>> keySelector,
        object value
    ) where TEntity : class, IQueryableEntity
    {
        if (options == null || keySelector == null)
            throw new ArgumentException("Can't add filter to null entity query options or with null key selector");

        // Get property name
        var memberExpression = keySelector.GetMemberExpression();
        var propertyName = memberExpression?.Member.Name ?? throw new InvalidOperationException();

        // TODO : Check value to property type
        options.FilterOptions = options.FilterOptions ?? new FilterOptions<TEntity>();
        options.FilterOptions.Filters.Add(new QueryFilter(propertyName, value?.ToString()));

        return options;
    }

    #endregion

    #region Adding include

    /// <summary>
    /// Adds include options to given query options
    /// </summary>
    /// <param name="options">The query options</param>
    /// <param name="keySelector">Model filter property selector</param>
    /// <typeparam name="TEntity">Query source type</typeparam>
    ///  <returns>Updated query options</returns>
    /// <exception cref="ArgumentNullException">If query options, key selector or value is null</exception>
    public static IEntityQueryOptions<TEntity> AddInclude<TEntity>(
        this IEntityQueryOptions<TEntity> options,
        Expression<Func<TEntity, IQueryableEntity>> keySelector
    ) where TEntity : class, IQueryableEntity
    {
        if (options == null || keySelector == null)
            throw new ArgumentException("Can't add include to null entity query options or with null key selector");

        // Get property name
        var memberExpression = keySelector.GetMemberExpression();
        var propertyName = memberExpression?.Member.Name ?? throw new InvalidOperationException();

        // TODO : Check value to property type
        options.IncludeOptions = options.IncludeOptions ?? new IncludeOptions<TEntity>();
        options.IncludeOptions.IncludeModels.Add(propertyName);

        return options;
    }

    /// <summary>
    /// Adds include options to given query options
    /// </summary>
    /// <param name="options">The query options</param>
    /// <param name="keySelector">Model filter property selector</param>
    /// <typeparam name="TEntity">Query source type</typeparam>
    ///  <returns>Updated query options</returns>
    /// <exception cref="ArgumentNullException">If query options, key selector or value is null</exception>
    public static IEntityQueryOptions<TEntity> AddInclude<TEntity>(
        this IEntityQueryOptions<TEntity> options,
        Expression<Func<TEntity, IEnumerable<IQueryableEntity>>> keySelector
    ) where TEntity : class, IQueryableEntity
    {
        if (options == null || keySelector == null)
            throw new ArgumentException("Can't add include to null query options or with null key selector");

        // Get property name
        var memberExpression = keySelector.GetMemberExpression();
        var propertyName = memberExpression?.Member.Name ?? throw new InvalidOperationException();

        // TODO : Check value to property type
        options.IncludeOptions = options.IncludeOptions ?? new IncludeOptions<TEntity>();
        options.IncludeOptions.IncludeModels.Add(propertyName);

        return options;
    }

    #endregion

    #region Adding sort

    /// <summary>
    /// Adds sort options to given query options
    /// </summary>
    /// <param name="options">The query options</param>
    /// <param name="keySelector">Model sort property selector</param>
    /// <param name="sortAscending">Value to filter with</param>
    /// <typeparam name="TModel">Query source type</typeparam>
    ///  <returns>Updated query options</returns>
    /// <exception cref="ArgumentNullException">If query options, key selector or value is null</exception>
    public static IQueryOptions<TModel> AddSort<TModel>(
        this IQueryOptions<TModel> options,
        Expression<Func<TModel, object>> keySelector,
        bool sortAscending
    ) where TModel : class
    {
        if (options == null || keySelector == null)
            throw new ArgumentException("Can't add sort to null query options or with null key selector");

        // Get property name
        var memberExpression = keySelector.GetMemberExpression();
        var propertyName = memberExpression?.Member.Name ?? throw new InvalidOperationException();

        options.SortOptions = new SortOptions<TModel>(propertyName, sortAscending);
        return options;
    }

    /// <summary>
    /// Adds sort options to given query options
    /// </summary>
    /// <param name="options">The query options</param>
    /// <param name="keySelector">Model sort property selector</param>
    /// <param name="sortAscending">Value to filter with</param>
    /// <typeparam name="TEntity">Query source type</typeparam>
    ///  <returns>Updated query options</returns>
    /// <exception cref="ArgumentNullException">If query options, key selector or value is null</exception>
    public static IEntityQueryOptions<TEntity> AddSort<TEntity>(
        this IEntityQueryOptions<TEntity> options,
        Expression<Func<TEntity, object>> keySelector,
        bool sortAscending
    ) where TEntity : class, IQueryableEntity
    {
        if (options == null || keySelector == null)
            throw new ArgumentException("Can't add sort to null entity query options or with null key selector");

        // Get property name
        var memberExpression = keySelector.GetMemberExpression();
        var propertyName = memberExpression?.Member.Name ?? throw new InvalidOperationException();

        options.SortOptions = new SortOptions<TEntity>(propertyName, sortAscending);
        return options;
    }

    #endregion

    #region Adding pagination

    /// <summary>
    /// Adds pagination options to given query options
    /// </summary>
    /// <param name="options">The query options</param>
    /// <param name="pageSize">Determines how many items should be selected</param>
    /// <param name="pageToken">Determines which section of items should be returned</param>
    /// <typeparam name="TModel">Query source type</typeparam>
    ///  <returns>Updated query options</returns>
    /// <exception cref="ArgumentNullException">If query options, key selector or value is null</exception>
    public static IQueryOptions<TModel> AddPagination<TModel>(this IQueryOptions<TModel> options, int pageSize, int pageToken) where TModel : class
    {
        if (options == null)
            throw new ArgumentException("Can't add pagination to null query options");

        options.PaginationOptions = new PaginationOptions(pageSize, pageToken);
        return options;
    }

    /// <summary>
    /// Adds pagination options to given query options
    /// </summary>
    /// <param name="options">The query options</param>
    /// <param name="pageSize">Determines how many items should be selected</param>
    /// <param name="pageToken">Determines which section of items should be returned</param>
    /// <typeparam name="TEntity">Query source type</typeparam>
    ///  <returns>Updated query options</returns>
    /// <exception cref="ArgumentNullException">If query options, key selector or value is null</exception>
    public static IEntityQueryOptions<TEntity> AddPagination<TEntity>(this IEntityQueryOptions<TEntity> options, int pageSize, int pageToken)
        where TEntity : class, IQueryableEntity
    {
        if (options == null)
            throw new ArgumentException("Can't add pagination to null entity query options");

        options.PaginationOptions = new PaginationOptions(pageSize, pageToken);
        return options;
    }

    #endregion
}