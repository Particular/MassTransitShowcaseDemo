namespace Helper
{
    using MassTransit;

    public class LowerCaseNameFormatter : DefaultEndpointNameFormatter
    {
        public string FormatEntityName<T>()
        {
            // Convert the entity name to lowercase
            return typeof(T).Name.ToLowerInvariant();
        }
    }

}
