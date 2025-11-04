namespace Contracts.Order.Response
{
    public class OrderItemResponse
    {
        public int OrderId { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? Username { get; set; }

        public List<OrderItemResponse> Items { get; set; } = null!;

        public decimal Total => Items.Sum(i => i.Subtotal);

        public decimal Subtotal { get; private set; }
    }
}
