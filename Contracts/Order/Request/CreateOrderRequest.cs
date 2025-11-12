using System.ComponentModel.DataAnnotations;

namespace Contracts.Order.Request
{
    public class CreateOrderRequest
    {
        [Required(ErrorMessage = "La orden debe contener al menos un item.")]
        [MinLength(1, ErrorMessage = "La orden debe contener al menos un item.")]
        public List<CreateOrderItemRequest> Items { get; set; } = new List<CreateOrderItemRequest>();
    }
}
