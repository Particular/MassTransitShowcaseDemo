namespace Sales;

using Messages;
using Microsoft.AspNetCore.SignalR;

public class SimulationEffects(IHubContext<SalesHub> salesHub)
{
    public event EventHandler RateChanged;
    public double FailureRate { get; private set; }
    public TimeSpan BaseProcessingTime { get; private set; } = TimeSpan.FromMilliseconds(1300);

    public void WriteState(TextWriter output)
    {
        output.WriteLine("Base time to handle each order: {0} seconds", BaseProcessingTime.TotalSeconds);
        output.WriteLine("Failure rate: {0:P0}", FailureRate);
    }

    public async Task IncreaseFailureRate()
    {
        FailureRate = Math.Min(1, FailureRate + failureRateIncrement);
        await NotifyOfRateChange();
    }

    public async Task DecreaseFailureRate()
    {
        FailureRate = Math.Max(0, FailureRate - failureRateIncrement);
        await NotifyOfRateChange();
    }

    public async Task SimulateMessageProcessing(CancellationToken cancellationToken = default)
    {
        try
        {
            if (Random.Shared.NextDouble() < FailureRate)
            {
                messagesErrored++;
                throw new Exception("BOOM! A failure occurred");
            }

            messagesProcessed++;
            await Task.Delay(BaseProcessingTime, cancellationToken);
        }
        finally
        {
            await salesHub.Clients.All.SendAsync("MessagesProcessed", messagesProcessed, messagesErrored, cancellationToken);
        }
    }

    public async Task ProcessMessagesFaster()
    {
        if (BaseProcessingTime > TimeSpan.Zero)
        {
            BaseProcessingTime -= increment;
            await NotifyOfRateChange();
        }
    }

    public async Task ProcessMessagesSlower()
    {
        BaseProcessingTime += increment;
        await NotifyOfRateChange();
    }

    public async Task Reset()
    {
        FailureRate = 0;
        BaseProcessingTime = TimeSpan.Zero;
        await NotifyOfRateChange();
    }

    async Task NotifyOfRateChange()
    {
        await salesHub.Clients.All.SendAsync("FailureRateChanged", Math.Round(FailureRate * 100, 0));
        await salesHub.Clients.All.SendAsync("ProcessingTimeChanged", BaseProcessingTime.TotalSeconds);
        RateChanged?.Invoke(this, EventArgs.Empty);
    }

    TimeSpan increment = TimeSpan.FromMilliseconds(100);
    const double failureRateIncrement = 0.1;
    int messagesProcessed = 0;
    int messagesErrored = 0;
}