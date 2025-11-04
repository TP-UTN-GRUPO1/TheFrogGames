namespace Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int GameId { get; set; }
    public Game Game { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
