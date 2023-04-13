using Tigernet.Hosting.Enums;

namespace Tigernet.Hosting.Attributes.RequestContents;

public class FromBodyAttribute : RequestContentAttribute
{
    /// <inheritdoc />
    public override bool CanMultiple { get; protected set; } = false;

    public override Transfer DataMapSource { get; protected set; } = Transfer.FromBody;
}
