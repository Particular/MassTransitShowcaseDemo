namespace Messages;

public record OrderPlaced
{
    public string OrderId { get; init; }
    public string[] Contents { get; init; }
}