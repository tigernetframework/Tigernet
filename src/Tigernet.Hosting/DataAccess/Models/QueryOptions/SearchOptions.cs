namespace Tigernet.Hosting.DataAccess.Models.QueryOptions;

/// <summary>
/// Represents search options
/// </summary>
/// <typeparam name="TModel">Query source type</typeparam>
public class SearchOptions<TModel> where TModel : class
{
    public SearchOptions(string keyword, bool includeChildren) =>
        (Keyword, IncludeChildren) = (keyword ?? throw new ArgumentException(), includeChildren);

    /// <summary>
    /// Search keyword
    /// </summary>
    public string Keyword { get; init; }

    /// <summary>
    /// Determines whether to search from direct children
    /// </summary>
    public bool IncludeChildren { get; init; }
}