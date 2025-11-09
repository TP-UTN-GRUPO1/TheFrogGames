namespace Contracts.Responses
{
    public class OrderItemResponse
    {
        public int Id { get; set; }
        public string? GameTitle { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
