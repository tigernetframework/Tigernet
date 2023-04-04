namespace Tigernet.Hosting.Models.Common;

public interface IEntity<TKey>
{
    TKey Id { get; set; }
}