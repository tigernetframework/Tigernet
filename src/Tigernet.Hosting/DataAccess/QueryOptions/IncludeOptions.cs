using Tigernet.Hosting.DataAccess.Entity;

namespace Tigernet.Hosting.DataAccess.QueryOptions;

/// <summary>
/// Represents table including options
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class IncludeOptions<TEntity> where TEntity : class, IQueryableEntity
{
    public IncludeOptions()
    {
        IncludeModels = new List<string>();
    }

    public IncludeOptions(List<string> includeModels)
    {
        IncludeModels = includeModels;
    }

    public List<string> IncludeModels { get; set; }
}