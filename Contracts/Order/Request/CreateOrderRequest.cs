using System.ComponentModel.DataAnnotations;

namespace Contracts.Order.Request
{
    public class CreateOrderRequest
    {
        public List<CreateOrderItemRequest> Items { get; set; } = new List<CreateOrderItemRequest>();


    }
}
