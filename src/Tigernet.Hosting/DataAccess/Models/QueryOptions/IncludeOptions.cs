using Tigernet.Hosting.DataAccess.Models.Entity;

namespace Tigernet.Hosting.DataAccess.Models.QueryOptions;

/// <summary>
/// Represents table including options
/// </summary>
/// <typeparam name="TEntity">Query source type</typeparam>
public class IncludeOptions<TEntity> where TEntity : class, IQueryableEntity
{
    public IncludeOptions()
    {
        IncludeModels = new List<string>();
    }

    public IncludeOptions(string includeModel)
    {
        IncludeModels = new List<string> { includeModel };
    }

    public List<string> IncludeModels { get; init; }
}