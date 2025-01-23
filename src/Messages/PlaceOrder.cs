namespace Messages;

public record PlaceOrder
{
    public string OrderId { get; init; }
    public string[] Contents { get; init; }
}