namespace Shipping
{
    public class OrderPlacedException : Exception
    {
        public OrderPlacedException(string message) : base(message)
        {
        }
    }
}
