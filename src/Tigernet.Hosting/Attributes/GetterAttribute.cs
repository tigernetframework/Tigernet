namespace Tigernet.Hosting.Attributes
{
    public class GetterAttribute : Attribute
    {
        public readonly string route;
        public GetterAttribute(string route = null)
        {
            this.route = route;
        }
    }
}
