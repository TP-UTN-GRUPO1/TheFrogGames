namespace Domain.Entities;

public class Order : BaseEntity
{
  
    public int UserId { get; set; }
    public User User { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal Total => Items.Sum(i => i.Quantity * i.UnitPrice);
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
