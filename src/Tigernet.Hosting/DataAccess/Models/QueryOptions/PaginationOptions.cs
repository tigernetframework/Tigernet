namespace Tigernet.Hosting.DataAccess.Models.QueryOptions;

/// <summary>
/// Represents pagination options
/// </summary>
public class PaginationOptions
{
    private int _pageSize;
    private int _pageToken;

    public PaginationOptions(int pageSize, int pageToken) => (PageSize, PageToken) = (pageSize, pageToken);

    /// <summary>
    /// Current page size
    /// </summary>
    /// <exception cref="ArgumentException">If value is invalid</exception>
    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = value <= 0 ? 20 : value;
    }

    /// <summary>
    /// Current page token
    /// </summary>
    /// <exception cref="ArgumentException">If value is invalid</exception>
    public int PageToken
    {
        get => _pageToken;
        init => _pageToken = value <= 0 ? 1 : value;
    }
}