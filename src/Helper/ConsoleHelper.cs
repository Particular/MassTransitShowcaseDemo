namespace Helper;

public class ConsoleHelper
{
#pragma warning disable PS0018
    public static async Task WriteMessageProcessed(DateTime sentTime)
#pragma warning restore PS0018
    {
        if (DateTime.UtcNow - sentTime >= TimeSpan.FromSeconds(10))
        {
            await Console.Out.WriteAsync("\x1b[91;40m.\x1b[0m");
        }
        else
        {
            await Console.Out.WriteAsync(".");
        }
    }
}