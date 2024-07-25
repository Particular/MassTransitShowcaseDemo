namespace Billing;

public class SimulationEffects
{
    public void IncreaseFailureRate()
    {
        failureRate = Math.Min(1, failureRate + failureRateIncrement);
    }

    public void DecreaseFailureRate()
    {
        failureRate = Math.Max(0, failureRate - failureRateIncrement);
    }

    public void WriteState(TextWriter output)
    {
        output.WriteLine("Failure rate: {0:P0}", failureRate);
    }

    public async Task SimulatedMessageProcessing(CancellationToken cancellationToken = default)
    {
        await Task.Delay(200, cancellationToken);

        if (Random.Shared.NextDouble() < failureRate)
        {
            throw new Exception("BOOM! A failure occurred");
        }
    }

    double failureRate;
    const double failureRateIncrement = 0.1;
}