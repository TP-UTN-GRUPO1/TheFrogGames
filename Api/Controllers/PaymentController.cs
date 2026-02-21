using Application.Abstraction.ExternalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Contracts.MercadoPago.Request;
using Contracts.MercadoPago.Response;
namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IMercadoPagoService _mercadoPagoService;

        public PaymentsController(IMercadoPagoService mercadoPagoService)
        {
            _mercadoPagoService = mercadoPagoService;
        }

        [HttpPost("create-preference")]
        public async Task<IActionResult> CreatePreference([FromBody] CreatePreferenceRequest request)
        {
            var preference = await _mercadoPagoService.CreatePreferenceAsync(request);

            return Ok(new { initPoint = preference.CheckoutUrl });
        }
    }
}
