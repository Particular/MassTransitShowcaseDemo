namespace Billing;

using Microsoft.AspNetCore.SignalR;

public class SimulationEffects(IHubContext<BillingHub> billingHub)
{
    public event EventHandler RateChanged;

    public async Task IncreaseFailureRate()
    {
        failureRate = Math.Min(1, failureRate + FailureRateIncrement);
        await NotifyOfRateChange();
    }

    public async Task DecreaseFailureRate()
    {
        failureRate = Math.Max(0, failureRate - FailureRateIncrement);
        await NotifyOfRateChange();
    }

    public void WriteState(TextWriter output) => output.WriteLine("Failure rate: {0:P0}", failureRate);

    public async Task SimulatedMessageProcessing(CancellationToken cancellationToken = default)
    {
        await Task.Delay(200, cancellationToken);

        if (Random.Shared.NextDouble() < failureRate)
        {
            throw new Exception("BOOM! A failure occurred");
        }
    }

    public async Task Reset()
    {
        failureRate = 0;
        await NotifyOfRateChange();
    }

    async Task NotifyOfRateChange()
    {
        await billingHub.Clients.All.SendAsync("RateChanged", (int)(failureRate * 100));
        RateChanged?.Invoke(this, EventArgs.Empty);
    }

    double failureRate;
    const double FailureRateIncrement = 0.1;
}