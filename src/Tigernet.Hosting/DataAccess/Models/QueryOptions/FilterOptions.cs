namespace Tigernet.Hosting.DataAccess.Models.QueryOptions;

/// <summary>
/// Represents filter options options
/// </summary>
/// <typeparam name="TModel">Query source type</typeparam>
public class FilterOptions<TModel> where TModel : class
{
    public FilterOptions() => (Filters) = new List<QueryFilter>();

    public FilterOptions(string key, string? value) =>
        (Filters) = new List<QueryFilter>
        {
            new(key, value)
        }; 
    
    public List<QueryFilter> Filters { get; set; }

    public QueryFilter? this[string key]
    {
        get { return Filters.FirstOrDefault(x => x.Key == key); }
    }
}