namespace ServiceControl;

using System;
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
    static readonly UriCreationOptions addressUriCreationOptions = new();

    async Task IFilter<ConsumeContext>.Send(ConsumeContext context, IPipe<ConsumeContext> next)
    {
        var proxy = new RetryAcknowledgementProxy(context);

        var useRetryAcknowledgement = IsRetriedMessage(context, out var id, out var acknowledgementQueueAddress);

        await next.Send(proxy).ConfigureAwait(false);

        if (proxy.IsFaulted)
        {
            // not typically going to happen, exception should be thrown, but you can check other things as well
            return;
        }

        if (useRetryAcknowledgement)
        {
            await ConfirmSuccessfulRetry().ConfigureAwait(false);
        }

        async Task ConfirmSuccessfulRetry()
        {
            var endpoint = await context.GetSendEndpoint(acknowledgementQueueAddress).ConfigureAwait(false);

            //cfg.Send<Empty>(m => m.UseSerializer("application/json"));
            await endpoint.Send(
                markMessagesConsumed,
                c =>
                {
                    //NServiceBus non ISO1806 compliant format
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

    void IProbeSite.Probe(ProbeContext context)
    {
    }

    static bool IsRetriedMessage(ConsumeContext context, out string retryUniqueMessageId, out Uri retryAcknowledgementAddress)
    {
        var h = context.ReceiveContext.TransportHeaders;

        // check if the message is coming from a manual retry attempt
        if (h.TryGetHeader(RetryUniqueMessageIdHeaderKey, out var uniqueMessageId) &&
            // The SC version that supports the confirmation message also started to add the SC version header
            h.TryGetHeader(RetryConfirmationQueueHeaderKey, out var acknowledgementQueue))
        {
            retryUniqueMessageId = (string)uniqueMessageId;
            var retryAcknowledgementQueue = (string)acknowledgementQueue;
            if (!Uri.TryCreate(retryAcknowledgementQueue, addressUriCreationOptions, out retryAcknowledgementAddress))
            {
                throw new InvalidOperationException($"Header '{RetryConfirmationQueueHeaderKey}' value contains a non addressable value");
            }
            return true;
        }

        retryUniqueMessageId = null;
        retryAcknowledgementAddress = null;
        return false;
    }
}