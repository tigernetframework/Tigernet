namespace Tigernet.Hosting.DataAccess.QueryOptions;

/// <summary>
/// Represents search options
/// </summary>
public class SearchOptions<TModel> where TModel : class
{
    public SearchOptions(string keyword, bool includeChildren) =>
        (Keyword, IncludeChildren) = (keyword ?? throw new ArgumentException(), includeChildren);

    /// <summary>
    /// Search keyword
    /// </summary>
    public string Keyword { get; }

    /// <summary>
    /// Determines whether to search from direct children
    /// </summary>
    public bool IncludeChildren { get; }
}