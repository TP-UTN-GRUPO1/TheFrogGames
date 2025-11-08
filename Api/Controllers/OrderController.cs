using Microsoft.AspNetCore.Mvc;
using Contracts.Responses;
using Contracts.Order.Request;
using Application.Abstraction;

namespace Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private OrderResponse? newId;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public ActionResult<List<OrderResponse>> GetAll()
        {
            var list = _orderService.GetOrders();
            return Ok(list);
        }
        // Antiguo: [HttpGet("{id}")] Get(int id)
        [HttpGet("user/{userId}")] // <--- 1. Renombrar la ruta para la búsqueda por usuario
        public ActionResult<List<OrderResponse>> GetByUserId(int userId) // <--- 2. Cambiar nombre del método
        {
            var list = _orderService.GetOrdersByUser(userId); // Llamada de servicio correcta
            if (list == null || !list.Any()) // Si es una lista, verifica si está vacía
            {
                return NotFound();
            }
            return Ok(list);
        }

        // Nuevo: Este es el método que usaremos para CreatedAtAction
        [HttpGet("{id}")]
        public ActionResult<OrderResponse> Get(int id) // <--- Este debe buscar por Order ID
        {
            // Asegúrate de que tu servicio tenga un método como GetOrderById
            var dto = _orderService.GetOrderById(id);
            if (dto == null)
            {
                return NotFound();
            }
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult Create([FromBody] CreateOrderRequest request)
        {
            try
            {
                var newOrderResponse = _orderService.CreateOrder(request);

                if (newOrderResponse == null)
                {
                    return BadRequest("No se pudo crear la orden.");
                }

                // Corregido:
                return CreatedAtAction(
                    nameof(Get), // 1. Nombre EXACTO del método GET (Get)
                    new { id = newOrderResponse.Id }, // 2. Parámetros de la ruta: usar la propiedad .Id
                    newOrderResponse // 3. El cuerpo de la respuesta 
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                _orderService.DeleteOrder(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }

}