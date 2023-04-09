using System.Linq.Expressions;
using System.Reflection;
using Tigernet.Hosting.Attributes.Query;
using Tigernet.Hosting.DataAccess.QueryOptions;

namespace Tigernet.Hosting.DataAccess.Query;

/// <summary>
/// Extensions for type operations
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Determines whether type A is child of type B
    /// </summary>
    /// <param name="child">Type checked as child</param>
    /// <param name="parent">Type checked as parent</param>
    /// <returns>Result of the check, true if is child</returns>
    public static bool InheritsOrImplements(this Type child, Type parent)
    {
        var par = parent;
        return InheritsOrImplementsHalf(child, ref parent) || par.IsAssignableFrom(child);
    }

    /// <summary>
    /// Determines whether type A inherits or implements type B
    /// </summary>
    /// <param name="child">Type checked as derived or implementing type</param>
    /// <param name="parent">Type checked as parent or interface type</param>
    /// <returns>Result of the check, true if does inherit or implement</returns>
    private static bool InheritsOrImplementsHalf(Type child, ref Type parent)
    {
        parent = ResolveGenericTypeDefinition(parent);
        var currentChild = child.IsGenericType ? child.GetGenericTypeDefinition() : child;
        while (currentChild != typeof(object))
        {
            if (parent == currentChild || HasAnyInterfaces(parent, currentChild))
                return true;
            currentChild = currentChild.BaseType is { IsGenericType: true }
                ? currentChild.BaseType.GetGenericTypeDefinition()
                : currentChild.BaseType;
            if (currentChild == null)
                return false;
        }

        return false;
    }

    /// <summary>
    /// Determines whether type A implements type B as direct interface
    /// </summary>
    /// <param name="child">Type checked as implementing type</param>
    /// <param name="parent">Type checked as interface type</param>
    /// <returns>Result of the check, true if does implement</returns>
    private static bool HasAnyInterfaces(Type parent, Type child)
    {
        return child.GetInterfaces()
            .Any(childInterface =>
            {
                var currentInterface = childInterface.IsGenericType ? childInterface.GetGenericTypeDefinition() : childInterface;

                return currentInterface == parent;
            });
    }

    /// <summary>
    /// Gets generic type definition from a type
    /// </summary>
    /// <param name="parent">Type being resolved</param>
    /// <returns>Generic type</returns>
    private static Type ResolveGenericTypeDefinition(Type parent)
    {
        var shouldUseGenericType = !(parent.IsGenericType && parent.GetGenericTypeDefinition() != parent);
        if (parent.IsGenericType && shouldUseGenericType)
            parent = parent.GetGenericTypeDefinition();
        return parent;
    }

    /// <summary>
    /// Checks if type is simple
    /// </summary>
    /// <param name="type">Type to check</param>
    /// <returns>True if type is simple, otherwise false</returns>
    public static bool IsSimpleType(this Type type)
    {
        return type.IsPrimitive || type == typeof(string) || type == typeof(DateTime) || type == typeof(DateTime?) || type == typeof(bool?);
    }

    /// <summary>
    /// Gets appropriate search method for a type
    /// </summary>
    /// <param name="type">Type in request</param>
    /// <param name="searchComparing">Determines whether to use search comparing methods</param>
    /// <returns>Method info of the compare method</returns>
    /// <exception cref="ArgumentException">If type is not primitive</exception>
    /// <exception cref="ArgumentNullException">If type is null</exception>
    /// <exception cref="InvalidOperationException">If not method found</exception>
    public static MethodInfo GetCompareMethod(this Type type, bool searchComparing = false)
    {
        if (type == null)
            throw new ArgumentNullException();

        if (!type.IsSimpleType())
            throw new ArgumentException("Not a primitive type");

        var methodName = type == typeof(string) && searchComparing ? "Contains" : "Equals";
        return type.GetMethod(methodName, new[] { type }) ?? throw new InvalidOperationException("Method not found");
    }

    /// <summary>
    /// Gets value in appropriate type in boxed format
    /// </summary>
    /// <param name="filter">Filter value</param>
    /// <param name="type">Type in request</param>
    /// <returns>Boxed filter value in its type</returns>
    /// <exception cref="ArgumentException">If type is not primitive</exception>
    /// <exception cref="ArgumentNullException">If filter or type is null</exception>
    /// <exception cref="InvalidOperationException">if no parse method found</exception>
    public static object GetValue(this QueryFilter filter, Type type)
    {
        if (filter == null || type == null)
            throw new ArgumentNullException();

        if (!type.IsSimpleType())
            throw new ArgumentException("Not a primitive type");

        // Return string or parsed value
        if (type == typeof(string))
        {
            return filter.Value;
        }
        else
        {
            // Create specific expression based on type
            var parameter = Expression.Parameter(typeof(string));
            var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
            var parseMethod = underlyingType.GetMethod("Parse", new[] { typeof(string) }) ??
                              throw new InvalidOperationException($"Method not found to parse value for type {type.FullName}");
            var argument = Expression.Constant(filter.Value);
            var methodCaller = Expression.Call(parseMethod, argument);
            var returnConverter = Expression.Convert(methodCaller, typeof(object));
            var function = Expression.Lambda<Func<string, object>>(returnConverter, parameter).Compile();
            return function.Invoke(filter.Value);
        }
    }

    public static IEnumerable<PropertyInfo> GetSearchableProperties(this Type type)
    {
        if (type == null)
            throw new ArgumentNullException();

        return type.GetProperties().Where(x => x.PropertyType.IsSimpleType() && Attribute.IsDefined(x, typeof(SearchablePropertyAttribute)));
    }

    public static IEnumerable<Type> GetCollectionUnderlyingType(this Type type)
    {
        if (null == type)
            throw new ArgumentNullException(nameof(type));

        return type.GetGenericArguments().ToList();
    }
}