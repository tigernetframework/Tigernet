using System.Linq.Expressions;

namespace Tigernet.Hosting.Extensions;

/// <summary>
/// Extends <see cref="Expression"/> base and other types
/// </summary>
public static class ExpressionExtensions
{
    public static MemberExpression GetMemberExpression<TModel, TKey>(this Expression<Func<TModel, TKey>> keySelector) where TModel : class
    {
        return keySelector.Body.NodeType switch
        {
            ExpressionType.Convert => ((UnaryExpression)keySelector.Body).Operand as MemberExpression,
            ExpressionType.MemberAccess => keySelector.Body as MemberExpression,
            _ => throw new ArgumentException("Not a member access", nameof(keySelector))
        };
    }
};