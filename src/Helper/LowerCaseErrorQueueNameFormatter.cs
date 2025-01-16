namespace Helper
{
    using MassTransit;


    public class LowerCaseErrorQueueNameFormatter : IErrorQueueNameFormatter
    {
       
        public string FormatErrorQueueName(string queueName)
        {
            return queueName.ToLowerInvariant();
        }
    }
}
