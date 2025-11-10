namespace Contracts.Responses
{
    public class OrderItemResponse
    {
        public string? GameTitle { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
