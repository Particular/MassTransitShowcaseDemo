namespace Helper;

using System.Security.Cryptography;
using System.Text;
using static MassTransit.MessageHeaders;

public class ConsoleHelper
{
#pragma warning disable PS0018
    public static Task WriteMessageProcessed(DateTime sentTime)
#pragma warning restore PS0018
    {
        lock (Console.Out)
        {
            if (DateTime.UtcNow - sentTime >= TimeSpan.FromSeconds(10))
            {
                Console.Write("\x1b[92m■\x1b[0m");
            }
            else
            {
                Console.Write("·");
            }
        }

        return Task.CompletedTask;
    }
    public static Guid DeterministicGuidBuilder(string input)
    {
        var inputBytes = Encoding.Default.GetBytes(input);

        // use MD5 hash to get a 16-byte hash of the string
        var hashBytes = MD5.HashData(inputBytes);

        // generate a guid from the hash:
        var g = new Guid(hashBytes);

        return g;
    }

    public static Guid MakeId(string data1, string data2)
    {
        return DeterministicGuidBuilder($"{data1}{data2}");
    }    

    public static Guid GetTheViewId(string name, string hostId) => MakeId(name, hostId);
}