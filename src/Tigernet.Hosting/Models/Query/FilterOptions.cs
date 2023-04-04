using Tigernet.Hosting.Models.Query;

namespace Tigernet.Hosting.Models.Query;

/// <summary>
/// Represents filtering options
/// </summary>
/// <typeparam name="TModel"></typeparam>
public class FilterOptions<TModel> where TModel : class
{
    public FilterOptions() => (Filters) = new List<QueryFilter>();

    public FilterOptions(IEnumerable<QueryFilter> filters) => (Filters) = filters.ToList();

    public List<QueryFilter> Filters { get; set; }

    public QueryFilter this[string key]
    {
        get { return Filters.FirstOrDefault(x => x.Key == key); }
    }
}