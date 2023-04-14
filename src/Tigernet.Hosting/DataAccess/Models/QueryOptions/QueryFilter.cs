namespace Tigernet.Hosting.DataAccess.Models.QueryOptions;

/// <summary>
/// Represents filter options unit
/// </summary>
public class QueryFilter
{
    public QueryFilter(string key, string? value) => (Key, Value) = (key, value);

    /// <summary>
    /// Field key name
    /// </summary>
    public string Key { get; init; }

    /// <summary>
    /// Filtering value
    /// </summary>
    public string? Value { get; init; }
}