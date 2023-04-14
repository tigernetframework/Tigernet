namespace Tigernet.Hosting.DataAccess.Models.QueryOptions;

/// <summary>
/// Represents collection sort options
/// </summary>
/// <typeparam name="TModel"></typeparam>
public class SortOptions<TModel>
{
    public SortOptions(string orderBy, bool orderAscending = true) => (OrderBy, OrderAscending) = (orderBy, orderAscending);

    /// <summary>
    /// Sort field
    /// </summary>
    public string OrderBy { get; init; }

    /// <summary>
    /// Indicates whether to sort ascending
    /// </summary>
    public bool OrderAscending { get; init; }
}