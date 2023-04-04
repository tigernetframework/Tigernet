using System.Linq.Expressions;

namespace Tigernet.Hosting.Models.Query;

/// <summary>
/// Provides extensions to build predicate expressions
/// </summary>
/// <typeparam name="TModel">Expression source</typeparam>
public static class PredicateBuilder<TModel> where TModel : class
{
    /// <summary>
    /// Initial true expression
    /// </summary>
    public static Expression<Func<TModel, bool>> True = entity => true;

    /// <summary>
    /// Initial false expression
    /// </summary>
    public static Expression<Func<TModel, bool>> False = entity => false;

    /// <summary>
    /// Joins to expression with OR logic
    /// </summary>
    /// <param name="left">Left expression</param>
    /// <param name="right">Right expression</param>
    /// <returns>Joined expression</returns>
    public static Expression<Func<TModel, bool>> Or(Expression<Func<TModel, bool>> left, Expression<Func<TModel, bool>> right)
    {
        if (left == null || right == null)
            throw new ArgumentException("Can't join null predicate expressions for OR operation");

        var invokeExpr = Expression.Invoke(right, left.Parameters.Cast<Expression>());
        return Expression.Lambda<Func<TModel, bool>>(Expression.OrElse(left.Body, invokeExpr), left.Parameters);
    }

    /// <summary>
    /// Joins to expression with AND logic
    /// </summary>
    /// <param name="left">Left expression</param>
    /// <param name="right">Right expression</param>
    /// <returns>Joined expression</returns>
    public static Expression<Func<TModel, bool>> And(Expression<Func<TModel, bool>> left, Expression<Func<TModel, bool>> right)
    {
        if (left == null || right == null)
            throw new ArgumentException("Can't join null predicate expressions for AND operation");

        var invokeExpr = Expression.Invoke(right, left.Parameters.Cast<Expression>());
        return Expression.Lambda<Func<TModel, bool>>(Expression.AndAlso(left.Body, invokeExpr), left.Parameters);
    }
}