namespace ServiceControl;

using System.Globalization;
using MassTransit;

record Dummy
{
}

class RetryAcknowledgementFilter : IFilter<ConsumeContext>
{
    // https://github.com/Particular/NServiceBus/blob/9.2.2/src/NServiceBus.Core/ServicePlatform/Retries/RetryAcknowledgementBehavior.cs#L12
    internal const string RetryUniqueMessageIdHeaderKey = "ServiceControl.Retry.UniqueMessageId";
    internal const string RetryConfirmationQueueHeaderKey = "ServiceControl.Retry.AcknowledgementQueue";

    // https://github.com/Particular/NServiceBus/blob/9.2.2/src/NServiceBus.Core/Headers.cs#L67
    public const string ControlMessageHeader = "NServiceBus.ControlMessage";

    static readonly Dummy markMessagesConsumed = new();

    public async Task Send(ConsumeContext context, IPipe<ConsumeContext> next)
    {
        var proxy = new RetryAcknowledgementProxy(context);

        var useRetryAcknowledgement = IsRetriedMessage(context, out var id, out var acknowledgementQueue);

        await next.Send(proxy).ConfigureAwait(false);

        if (proxy.IsFaulted)
        {
            // not typically going to happen, exception should be thrown, but you can check other things as well
        }

        if (proxy.Consumed.Any())
        {
            if (useRetryAcknowledgement)
            {
                await ConfirmSuccessfulRetry().ConfigureAwait(false);
            }

            async Task ConfirmSuccessfulRetry()
            {
                // TODO: Hardcoded to `exchange:`, likely this needs to go through the connector too and detect that its a ACK and 
                var address = new Uri("exchange:" + acknowledgementQueue);
                var endpoint = await context.GetSendEndpoint(address).ConfigureAwait(false);

                //cfg.Send<Empty>(m => m.UseSerializer("application/json"));
                await endpoint.Send(
                    markMessagesConsumed,
                    c =>
                    {
                        //NServiceBUs non ISO1806 compliant format
                        var timestamp = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss:ffffff Z", CultureInfo.InvariantCulture);

                        var h = c.Headers;
                        h.Set("ServiceControl.Retry.Successful", timestamp);
                        h.Set(RetryUniqueMessageIdHeaderKey, id);
                        h.Set(ControlMessageHeader, bool.TrueString);
                        return Task.CompletedTask;
                    }
                    ).ConfigureAwait(false);
            }
        }
    }

    public void Probe(ProbeContext context)
    {
    }

    static bool IsRetriedMessage(ConsumeContext context, out string retryUniqueMessageId, out string retryAcknowledgementQueue)
    {
        var h = context.ReceiveContext.TransportHeaders;

        // check if the message is coming from a manual retry attempt
        if (h.TryGetHeader(RetryUniqueMessageIdHeaderKey, out var uniqueMessageId) &&
            // The SC version that supports the confirmation message also started to add the SC version header
            h.TryGetHeader(RetryConfirmationQueueHeaderKey, out var acknowledgementQueue))
        {
            retryUniqueMessageId = (string)uniqueMessageId;
            retryAcknowledgementQueue = (string)acknowledgementQueue;
            return true;
        }

        retryUniqueMessageId = null;
        retryAcknowledgementQueue = null;
        return false;
    }
}
