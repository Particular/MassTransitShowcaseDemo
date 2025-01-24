namespace Billing
{
    using System;

    public class BillingProcessingException : Exception
    {
        public BillingProcessingException(string message) : base(message)
        {
        }
    }
}
