namespace Contracts.Responses
{
    public class OrderItemResponse
    {
        public int GameId { get; set; }
        public string? GameTitle { get; set; }
        public string? Developer { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public float Rating { get; set; }
        public bool Available { get; set; }
        public int Sold { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }
}
