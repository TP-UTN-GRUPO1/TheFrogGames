namespace Contracts.Responses
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string OrderDate { get; set; } = string.Empty;
        public decimal Total { get; set; }

        public List<OrderItemResponse> Items { get; set; } = new List<OrderItemResponse>();
        public bool IsCancelled { get; set; } = false;
        public string CheckoutUrl { get; set; }
    }
}