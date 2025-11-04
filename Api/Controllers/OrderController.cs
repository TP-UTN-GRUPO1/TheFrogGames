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
        [HttpGet("{id}")]
        public ActionResult<OrderItemResponse> Get(int id)
        {
            var dto = _orderService.GetOrdersByUser(id);
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
                newId = _orderService.CreateOrder(request);
                return CreatedAtAction(nameof(Get), new { id =  newId }, null);
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