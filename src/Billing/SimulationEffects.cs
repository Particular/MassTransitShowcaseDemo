namespace Billing;

using Microsoft.AspNetCore.SignalR;

public class SimulationEffects(IHubContext<BillingHub> billingHub)
{
    public event EventHandler RateChanged;
    public double FailureRate { get; private set; }

    public async Task IncreaseFailureRate()
    {
        FailureRate = Math.Min(1, FailureRate + FailureRateIncrement);
        await NotifyOfRateChange();
    }

    public async Task DecreaseFailureRate()
    {
        FailureRate = Math.Max(0, FailureRate - FailureRateIncrement);
        await NotifyOfRateChange();
    }

    public void WriteState(TextWriter output) => output.WriteLine("Failure rate: {0:P0}", FailureRate);

    public async Task SimulatedMessageProcessing(CancellationToken cancellationToken = default)
    {
        await Task.Delay(200, cancellationToken);

        if (Random.Shared.NextDouble() < FailureRate)
        {
            throw new Exception("BOOM! A failure occurred");
        }
    }

    public async Task Reset()
    {
        FailureRate = 0;
        await NotifyOfRateChange();
    }

    async Task NotifyOfRateChange()
    {
        await billingHub.Clients.All.SendAsync("FailureRateChanged", Math.Round(FailureRate * 100, 0));
        RateChanged?.Invoke(this, EventArgs.Empty);
    }

    const double FailureRateIncrement = 0.1;
}