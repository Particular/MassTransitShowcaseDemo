using MassTransit;
using MassTransit.Context;

class RetryAcknowledgementProxy(ConsumeContext context) : ConsumeContextProxy(context)
{
    public bool IsFaulted { get; private set; }

    public override Task NotifyFaulted<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType, Exception exception)
    {
        IsFaulted = true;

        return base.NotifyFaulted(context, duration, consumerType, exception);
    }
}