using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Tigernet.Hosting.DataAccess.Entity;
using Tigernet.Hosting.DataAccess.QueryOptions;

namespace Tigernet.Hosting.DataAccess.Query;

public static class QueryExtensions
{
    #region Querying

    /// <summary>
    /// Applies given query options to simple enumerable source
    /// </summary>
    /// <param name="source">Enumerable source</param>
    /// <param name="queryOptions">Query options</param>
    /// <typeparam name="TModel">Query source type</typeparam>
    /// <returns>Enumerable source</returns>
    public static IEnumerable<TModel> ApplyQuery<TModel>(this IEnumerable<TModel> source, IQueryOptions<TModel> queryOptions) where TModel : class
    {
        if (source == null || queryOptions == null)
            throw new ArgumentException("Cannot apply query options to null source or null query options");

        var result = source;

        if (queryOptions.SearchOptions != null)
            result = result.ApplySearch(queryOptions.SearchOptions);

        if (queryOptions.FilterOptions != null)
            result = result.ApplyFilter(queryOptions.FilterOptions);

        if (queryOptions.SortOptions != null)
            result = result.ApplySort(queryOptions.SortOptions);

        result = result.ApplyPagination(queryOptions.PaginationOptions);

        return result;
    }

    /// <summary>
    /// Applies given query options to simple query source
    /// </summary>
    /// <param name="source">Query source</param>
    /// <param name="queryOptions">Query options</param>
    /// <typeparam name="TModel">Query source type</typeparam>
    /// <returns>Queryable source</returns>
    public static IQueryable<TModel> ApplyQuery<TModel>(this IQueryable<TModel> source, IQueryOptions<TModel> queryOptions) where TModel : class
    {
        if (source == null || queryOptions == null)
            throw new ArgumentException("Cannot apply query options to null source or null query options");

        var result = source;

        if (queryOptions.SearchOptions != null)
            result = result.ApplySearch(queryOptions.SearchOptions);

        if (queryOptions.FilterOptions != null)
            result = result.ApplyFilter(queryOptions.FilterOptions);

        if (queryOptions.SortOptions != null)
            result = result.ApplySort(queryOptions.SortOptions);

        result = result.ApplyPagination(queryOptions.PaginationOptions);

        return result;
    }

    /// <summary>
    /// Applies given query options to relational query source
    /// </summary>
    /// <param name="source">Query source</param>
    /// <param name="queryOptions">Query options</param>
    /// <typeparam name="TEntity">Query source type</typeparam>
    /// <returns>Queryable source</returns>
    public static IQueryable<TEntity> ApplyQuery<TEntity>(this IQueryable<TEntity> source, IEntityQueryOptions<TEntity> queryOptions)
        where TEntity : class, IQueryableEntity
    {
        if (source == null || queryOptions == null)
            throw new ArgumentException("Cannot apply query options to null source or null query options");

        var result = source;

        if (queryOptions.SearchOptions != null)
            result = result.ApplySearch(queryOptions.SearchOptions);

        if (queryOptions.FilterOptions != null)
            result = result.ApplyFilter(queryOptions.FilterOptions);

        if (queryOptions.SortOptions != null)
            result = result.ApplySort(queryOptions.SortOptions);

        result = result.ApplyPagination(queryOptions.PaginationOptions);

        return result;
    }

    #endregion

    #region Searching

    /// <summary>
    /// Creates expression from search options
    /// </summary>
    /// <param name="searchOptions">Search options</param>
    /// <typeparam name="TModel">Query source type</typeparam>
    /// <returns>Queryable source</returns>
    private static Expression<Func<TModel, bool>> GetSearchExpression<TModel>(this SearchOptions<TModel> searchOptions) where TModel : class
    {
        if (searchOptions == null)
            throw new ArgumentException("Can't create search expressions for null search options");

        // Get the properties type of entity
        var parameter = Expression.Parameter(typeof(TModel));
        var searchableProperties = typeof(TModel).GetSearchableProperties();

        // Add searchable properties
        var predicates = searchableProperties?.Select(x =>
            {
                // Create predicate expression
                var member = Expression.PropertyOrField(parameter, x.Name);

                // Create specific expression based on type
                var compareMethod = x.PropertyType.GetCompareMethod(true);
                var argument = Expression.Constant(searchOptions.Keyword, x.PropertyType);
                var methodCaller = Expression.Call(member, compareMethod!, argument);
                return Expression.Lambda<Func<TModel, bool>>(methodCaller, parameter);
            })
            .ToList();

        // Join predicate expressions
        var finalExpression = PredicateBuilder<TModel>.False;
        predicates?.ForEach(x => finalExpression = PredicateBuilder<TModel>.Or(finalExpression, x));

        return finalExpression;
    }

    /// <summary>
    /// Applies given searching options to query source
    /// </summary>
    /// <param name="source">Query source</param>
    /// <param name="searchOptions">Search options</param>
    /// <typeparam name="TModel">Query source type</typeparam>
    /// <returns>Queryable source</returns>
    public static IEnumerable<TModel> ApplySearch<TModel>(this IEnumerable<TModel> source, SearchOptions<TModel> searchOptions) where TModel : class
    {
        if (source == null || searchOptions == null)
            throw new ArgumentException("Can't apply search to null source or with null search options");

        return source.Where(searchOptions.GetSearchExpression().Compile());
    }

    /// <summary>
    /// Applies given searching options to query source
    /// </summary>
    /// <param name="source">Query source</param>
    /// <param name="searchOptions">Search options</param>
    /// <typeparam name="TModel">Query source type</typeparam>
    /// <returns>Queryable source</returns>
    public static IQueryable<TModel> ApplySearch<TModel>(this IQueryable<TModel> source, SearchOptions<TModel> searchOptions) where TModel : class
    {
        if (source == null || searchOptions == null)
            throw new ArgumentException("Can't apply search to null source or with null search options");

        var searchExpressions = searchOptions.GetSearchExpression();

        // Include direct child entities if they have searchable properties too
        if (source is IQueryable<IQueryableEntity> entitySource && searchOptions is SearchOptions<IQueryableEntity> entitySearchOptions &&
            searchExpressions is Expression<Func<IQueryableEntity, bool>> entitySearchExpressions)
            entitySearchExpressions.AddSearchIncludeExpressions(entitySearchOptions, entitySource);

        return source.Where(searchExpressions);
    }

    #endregion

    #region Filtering

    /// <summary>
    /// Creates expression from filter options
    /// </summary>
    /// <param name="filterOptions">Filters</param>
    /// <typeparam name="TModel">Query source type</typeparam>
    /// <returns>Queryable source</returns>
    public static Expression<Func<TModel, bool>> GetFilterExpression<TModel>(this FilterOptions<TModel> filterOptions) where TModel : class
    {
        if (filterOptions == null)
            throw new ArgumentException("Can't create filter expressions for null search options");

        // Get the properties type  of entity
        var parameter = Expression.Parameter(typeof(TModel));
        var properties = typeof(TModel).GetProperties().Where(x => x.PropertyType.IsSimpleType()).ToList();

        // Convert filters to predicate expressions
        var predicates = filterOptions.Filters.Where(x => properties.Any(y => y.Name.ToLower() == x.Key.ToLower()))
            .GroupBy(x => x.Key)
            .Select(x =>
            {
                // Create multi choice predicates
                var predicate = PredicateBuilder<TModel>.False;
                var multiChoicePredicates = x.Select(y =>
                    {
                        // Create predicate expression
                        var property = properties.First(z => string.Equals(z.Name, y.Key, StringComparison.CurrentCultureIgnoreCase));
                        var member = Expression.PropertyOrField(parameter, y.Key);

                        // Create specific expression based on type
                        var compareMethod = property.PropertyType.GetCompareMethod();
                        var expectedType = compareMethod.GetParameters().First();

                        var argument = Expression.Convert(Expression.Constant(y.GetValue(property.PropertyType)), expectedType.ParameterType);
                        var methodCaller = Expression.Call(member, compareMethod, argument);
                        return Expression.Lambda<Func<TModel, bool>>(methodCaller, parameter);
                    })
                    .ToList();

                multiChoicePredicates.ForEach(y => predicate = PredicateBuilder<TModel>.Or(predicate, y));
                return predicate;
            })
            .ToList();

        // Join predicate expressions
        var finalExpression = PredicateBuilder<TModel>.True;
        predicates.ForEach(x => finalExpression = PredicateBuilder<TModel>.And(finalExpression, x));

        return finalExpression;
    }

    /// <summary>
    /// Applies given filter options to query source
    /// </summary>
    /// <param name="source">Query source</param>
    /// <param name="filterOptions">Filter options</param>
    /// <typeparam name="TModel">Query source type</typeparam>
    /// <returns>Queryable source</returns>
    public static IEnumerable<TModel> ApplyFilter<TModel>(this IEnumerable<TModel> source, FilterOptions<TModel> filterOptions) where TModel : class
    {
        if (source == null || filterOptions == null)
            throw new ArgumentException("Can't apply filter to null source or with null filter options");

        return source.Where(filterOptions.GetFilterExpression().Compile());
    }

    public static IQueryable<TModel> ApplyFilter<TModel>(this IQueryable<TModel> source, FilterOptions<TModel> filterOptions) where TModel : class
    {
        if (source == null || filterOptions == null)
            throw new ArgumentException("Can't apply filter to null source or with null filter options");

        return source.Where(filterOptions.GetFilterExpression());
    }

    #endregion

    #region Sorting

    /// <summary>
    /// Applies given sorting options to query source
    /// </summary>
    /// <param name="source">Query source</param>
    /// <param name="sortOptions">Sort options</param>
    /// <typeparam name="TModel">Query source type</typeparam>
    /// <returns>Queryable source</returns>
    public static IEnumerable<TModel> ApplySort<TModel>(this IEnumerable<TModel> source, SortOptions<TModel> sortOptions) where TModel : class
    {
        if (source == null || sortOptions == null)
            throw new ArgumentException("Can't apply sort to null source or with null sort options");

        // Get the properties type  of entity
        var parameter = Expression.Parameter(typeof(TModel));
        var properties = typeof(TModel).GetProperties().Where(x => x.PropertyType.IsSimpleType()).ToList();

        // Apply sorting
        var matchingProperty = properties.FirstOrDefault(x => x.Name.ToLower() == sortOptions.SortField.ToLower());

        if (matchingProperty == null)
            return source;

        var memExp = Expression.PropertyOrField(parameter, matchingProperty.Name);
        var keySelector = Expression.Lambda<Func<TModel, object>>(memExp, true, parameter).Compile();

        return sortOptions.SortAscending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
    }

    /// <summary>
    /// Applies given sorting options to query source
    /// </summary>
    /// <param name="source">Query source</param>
    /// <param name="sortOptions">Sort options</param>
    /// <typeparam name="TModel">Query source type</typeparam>
    /// <returns>Queryable source</returns>
    public static IQueryable<TModel> ApplySort<TModel>(this IQueryable<TModel> source, SortOptions<TModel> sortOptions) where TModel : class
    {
        if (source == null || sortOptions == null)
            throw new ArgumentException("Can't apply sort to null source or with null sort options");

        // Get the properties type  of entity
        var parameter = Expression.Parameter(typeof(TModel));
        var properties = typeof(TModel).GetProperties().Where(x => x.PropertyType.IsSimpleType()).ToList();

        // Apply sorting
        var matchingProperty = properties.FirstOrDefault(x => x.Name.ToLower() == sortOptions.SortField.ToLower());

        if (matchingProperty == null)
            return source;

        var memExp = Expression.Convert(Expression.PropertyOrField(parameter, matchingProperty.Name), typeof(object));
        var keySelector = Expression.Lambda<Func<TModel, dynamic>>(memExp, true, parameter);
        return sortOptions.SortAscending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
    }

    #endregion

    #region Including

    private static Expression<Func<TEntity, bool>> AddSearchIncludeExpressions<TEntity>(
        this Expression<Func<TEntity, bool>> searchExpressions,
        SearchOptions<TEntity> searchOptions,
        IQueryable<TEntity> source
    ) where TEntity : class, IQueryableEntity
    {
        if (searchOptions == null || source == null)
            throw new ArgumentException("Can't create search include expressions to null source or with null search options");

        var relatedEntitiesProperty = typeof(TEntity).GetDirectChildEntities()
            ?.Select(x => new
            {
                Entity = x,
                SearchableProperties = x.GetSearchableProperties()
            });
        var matchingRelatedEntities = relatedEntitiesProperty?.Where(x => x.SearchableProperties.Any()).ToList();

        // Include models
        var predicates = matchingRelatedEntities?.Select(x =>
            {
                // Include matching entities
                source.Include(x.Entity.Name);

                // Add matching entity predicates
                var parameter = Expression.Parameter(typeof(TEntity));

                // Add searchable properties
                return x.SearchableProperties?.Select(y =>
                {
                    // Create predicate expression
                    var entity = Expression.PropertyOrField(parameter, x.Entity.Name);
                    var entityProperty = Expression.PropertyOrField(entity, y.Name);

                    // Create specific expression based on type
                    var compareMethod = y.PropertyType.GetCompareMethod(true);
                    var argument = Expression.Constant(searchOptions.Keyword, y.PropertyType);
                    var methodCaller = Expression.Call(entityProperty, compareMethod!, argument);
                    return Expression.Lambda<Func<TEntity, bool>>(methodCaller, parameter);
                });
            })
            .ToList();

        // Join predicate expressions
        predicates?.ForEach(x => x?.ToList().ForEach(y => searchExpressions = PredicateBuilder<TEntity>.Or(searchExpressions, y)));
        return searchExpressions;
    }

    /// <summary>
    /// Applies given include models options to query source
    /// </summary>
    /// <param name="source">Query source</param>
    /// <param name="includeOptions">Include models options</param>
    /// <typeparam name="TEntity">Query source type</typeparam>
    /// <returns>Queryable source</returns>
    public static IQueryable<TEntity> ApplyIncluding<TEntity>(this IQueryable<TEntity> source, IncludeOptions<TEntity> includeOptions)
        where TEntity : class, IQueryableEntity
    {
        if (source == null || includeOptions == null)
            throw new ArgumentException("Can't apply include to null source or with null include options");

        // Include models
        includeOptions.IncludeModels = includeOptions.IncludeModels.Select(x => x.ToLower()).ToList();
        var includeModels = typeof(TEntity).GetDirectChildEntities()
            .Where(x => includeOptions.IncludeModels.Any(y => x.Name.Equals(y, StringComparison.InvariantCultureIgnoreCase)))
            .ToList();

        includeModels.ForEach(x => { source = source.Include(x.Name); });
        return source;
    }

    #endregion

    #region Pagination

    /// <summary>
    /// Applies given sorting options to query source
    /// </summary>
    /// <param name="source">Query source</param>
    /// <param name="paginationOptions">Sort options</param>
    /// <typeparam name="TModel">Query source type</typeparam>
    /// <returns>Queryable source</returns>
    public static IEnumerable<TModel> ApplyPagination<TModel>(this IEnumerable<TModel> source, PaginationOptions paginationOptions)
    {
        if (source == null || paginationOptions == null)
            throw new ArgumentException("Can't apply pagination to null source or with null pagination options");

        return source.Skip((paginationOptions.PageToken - 1) * paginationOptions.PageSize).Take(paginationOptions.PageSize);
    }

    /// <summary>
    /// Applies given sorting options to query source
    /// </summary>
    /// <param name="source">Query source</param>
    /// <param name="paginationOptions">Sort options</param>
    /// <typeparam name="TModel">Query source type</typeparam>
    /// <returns>Queryable source</returns>
    public static IQueryable<TModel> ApplyPagination<TModel>(this IQueryable<TModel> source, PaginationOptions paginationOptions)
    {
        if (source == null || paginationOptions == null)
            throw new ArgumentException("Can't apply pagination to null source or with null pagination options");

        return source.Skip((paginationOptions.PageToken - 1) * paginationOptions.PageSize).Take(paginationOptions.PageSize);
    }

    #endregion
}