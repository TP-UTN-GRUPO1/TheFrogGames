using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Contracts.Responses;
using Contracts.Order.Request;
using Application.Abstraction;

namespace Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
        [HttpGet("user/{userId}")]
        public ActionResult<List<OrderResponse>> GetByUserId(int userId) 
        {
            var list = _orderService.GetOrdersByUser(userId); 
            if (list == null || !list.Any()) 
            {
                return NotFound();
            }
            return Ok(list);
        }

        [HttpGet("{id}")]
        public ActionResult<OrderResponse> Get(int id) 
        {
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