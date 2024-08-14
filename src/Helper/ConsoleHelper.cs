namespace Helper
{
    public class ConsoleHelper
    {
        public static void WriteMessageProcessed(DateTime sentTime)
        {
            if (DateTime.UtcNow - sentTime >= TimeSpan.FromSeconds(10))
            {
                Console.Write("\x1b[91;40m.\x1b[0m");
            }
            else
            {
                Console.Write(".");
            }
        }
    }
}
