namespace Messages;

public record OrderBilled
{
    public string OrderId { get; init; }
    public string[] Contents { get; init; }
}