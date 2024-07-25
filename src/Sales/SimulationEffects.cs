namespace Sales;

public class SimulationEffects
{
    public void WriteState(TextWriter output)
    {
        output.WriteLine("Base time to handle each order: {0} seconds", baseProcessingTime.TotalSeconds);
    }

    public Task SimulateMessageProcessing(CancellationToken cancellationToken = default)
    {
        return Task.Delay(baseProcessingTime, cancellationToken);
    }

    public void ProcessMessagesFaster()
    {
        if (baseProcessingTime > TimeSpan.Zero)
        {
            baseProcessingTime -= increment;
        }
    }

    public void ProcessMessagesSlower()
    {
        baseProcessingTime += increment;
    }

    TimeSpan baseProcessingTime = TimeSpan.FromMilliseconds(1300);
    TimeSpan increment = TimeSpan.FromMilliseconds(100);
}