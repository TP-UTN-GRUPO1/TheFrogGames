
using Contracts.MercadoPago.Request;
using Contracts.MercadoPago.Response;
namespace Application.Abstraction.ExternalServices
{
    public interface IMercadoPagoService
    {
        Task<CheckoutResponse> CreatePreferenceAsync(CreatePreferenceRequest request);
    }
}
