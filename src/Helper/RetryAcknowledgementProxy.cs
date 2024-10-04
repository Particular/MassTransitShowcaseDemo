using MassTransit;
using MassTransit.Context;

class RetryAcknowledgementProxy(ConsumeContext context) : ConsumeContextProxy(context)
{
    public bool IsFaulted { get; private set; }

    readonly List<Guid> _consumed = [];
    public IReadOnlyList<Guid> Consumed => _consumed;

    public override Task NotifyConsumed<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType)
    {
        if (context.MessageId.HasValue)
        {
            _consumed.Add(context.MessageId.Value);
        }

        return base.NotifyConsumed(context, duration, consumerType);
    }

    public override Task NotifyFaulted<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType, Exception exception)
    {
        IsFaulted = true;

        return base.NotifyFaulted(context, duration, consumerType, exception);
    }
}