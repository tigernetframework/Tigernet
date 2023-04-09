using Tigernet.Hosting.Models.Common;

namespace Tigernet.Hosting.Models.Query;

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