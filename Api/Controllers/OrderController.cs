using Application.Abstraction.ExternalServices;
using Contracts.Order.Request;
using Contracts.Responses;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IMercadoPagoQueryService _mercadoPagoQueryService;

        private readonly IOrderService _orderService;

        public OrdersController(
         IOrderService orderService,
         IMercadoPagoQueryService mercadoPagoQueryService)
        {
            _orderService = orderService;
            _mercadoPagoQueryService = mercadoPagoQueryService;
        }

        [HttpGet]
        public ActionResult<List<OrderResponse>> GetAll()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var tokenUserIdStr = User.FindFirst("idUser")?.Value;

            List<OrderResponse> list;
            if (role == nameof(TypeRole.SysAdmin) || role == nameof(TypeRole.Admin))
            {
                list = _orderService.GetOrders();
            }
            else
            {
                if (!int.TryParse(tokenUserIdStr, out int tokenUserId))
                    return StatusCode(403, "No autorizado");

                list = _orderService.GetOrdersByUser(tokenUserId);
            }

            if (list == null || !list.Any())
                return NotFound(new { message = "No se encontraron órdenes." });

            return Ok(list);
        }

        [HttpGet("user/{userId}")]
        public ActionResult<List<OrderResponse>> GetByUserId(int userId)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var tokenUserIdStr = User.FindFirst("idUser")?.Value;

            if (role != nameof(TypeRole.SysAdmin) && role != nameof(TypeRole.Admin))
            {
                if (!int.TryParse(tokenUserIdStr, out int tokenUserId) || tokenUserId != userId)
                    return StatusCode(403, "No tienes permisos para ver las órdenes de otro usuario");
            }

            var list = _orderService.GetOrdersByUser(userId);
            if (list == null || !list.Any())
            {
                return NotFound(new { message = $"No se encontraron órdenes para el usuario {userId}." });
            }
            return Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult<OrderResponse> Get(int id)
        {
            var dto = _orderService.GetOrderById(id);
            if (dto == null)
            {
                return NotFound(new { message = $"Orden con id {id} no encontrada." });
            }

            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var tokenUserIdStr = User.FindFirst("idUser")?.Value;

            if (role != nameof(TypeRole.SysAdmin) && role != nameof(TypeRole.Admin))
            {
                if (!int.TryParse(tokenUserIdStr, out int tokenUserId) || tokenUserId != dto.UserId)
                    return StatusCode(403, "No tienes permisos para ver esta orden");
            }

            return Ok(dto);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateOrderRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != nameof(TypeRole.User))
            {
                return StatusCode(403, "Solo un cliente normal puede hacer una orden de compra");
            }

            var tokenUserIdStr = User.FindFirst("idUser")?.Value;
            if (!int.TryParse(tokenUserIdStr, out int tokenUserId))
            {
                return StatusCode(403, "No autorizado");
            }

            try
            {
                var newOrderResponse = await _orderService.CreateOrder(request, tokenUserId);

                if (newOrderResponse == null)
                {
                    return BadRequest(new { message = "No se pudo crear la orden." });
                }

                return CreatedAtAction(
                    nameof(Get),
                    new { id = newOrderResponse.Id },
                    newOrderResponse
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
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                if (role != nameof(TypeRole.SysAdmin) && role != nameof(TypeRole.Admin))
                    return StatusCode(403, "No tienes permisos para eliminar órdenes");

                var deleted = _orderService.DeleteOrder(id);
                if (!deleted)
                    return NotFound(new { message = $"No se encontró la orden con id {id} para eliminar." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [AllowAnonymous]
        [HttpGet("verify")]
        public async Task<IActionResult> VerifyPayment([FromQuery] string paymentId)
        {
            if (string.IsNullOrEmpty(paymentId))
                return BadRequest();

            var payment = await _mercadoPagoQueryService.GetPaymentAsync(paymentId);

            if (payment.Status == "approved")
            {
                return Ok(new { approved = true });
            }

            return Ok(new { approved = false });
        }
    }

}