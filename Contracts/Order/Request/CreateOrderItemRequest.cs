namespace Contracts.Order.Request
{
    public class CreateOrderItemRequest
    {
        public int GameId { get; set; }
        public int Quantity { get; set; }
    }
}
