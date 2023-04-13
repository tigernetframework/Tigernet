using Tigernet.Hosting.Enums;

namespace Tigernet.Hosting.Attributes.RequestContents;

[AttributeUsage(AttributeTargets.Parameter)]
public abstract class RequestContentAttribute : Attribute
{
    /// <summary>
    /// Request coming data section can be multiple
    /// </summary>
    public abstract bool CanMultiple { get; protected set; }   
    
    public abstract Transfer DataMapSource { get; protected set; }
}
