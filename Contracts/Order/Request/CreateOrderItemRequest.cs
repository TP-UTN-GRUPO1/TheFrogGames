using System.ComponentModel.DataAnnotations;

namespace Contracts.Order.Request
{
    public class CreateOrderItemRequest
    {
        [Required(ErrorMessage = "El identificador del juego es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El identificador del juego debe ser un número positivo.")]
        public int GameId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int Quantity { get; set; }
    }
}
