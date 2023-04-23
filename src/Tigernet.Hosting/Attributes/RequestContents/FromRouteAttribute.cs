using Tigernet.Hosting.Enums;

namespace Tigernet.Hosting.Attributes.RequestContents
{
    public class FromRouteAttribute : RequestContentAttribute
    {
        /// <inheritdoc />
        public override bool CanMultiple { get; protected set; } = false;

        public override Transfer DataMapSource { get; protected set; } = Transfer.FromRoute;
    }
}
