namespace Tigernet.Hosting.DataAccess.QueryOptions;

/// <summary>
/// Represents collection sort options
/// </summary>
/// <typeparam name="TModel"></typeparam>
public class SortOptions<TModel>
{
    public SortOptions(string sortField, bool sortAscending = true) => (SortField, SortAscending) = (sortField, sortAscending);

    /// <summary>
    /// Sort field
    /// </summary>
    public string SortField { get; }

    /// <summary>
    /// Indicates whether to sort ascending
    /// </summary>
    public bool SortAscending { get; }
}