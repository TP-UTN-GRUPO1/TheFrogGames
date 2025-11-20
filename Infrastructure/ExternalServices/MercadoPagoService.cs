
using Application.Abstraction.ExternalServices;
using Contracts.MercadoPago.Response;
using System.Net.Http.Json;

namespace Infrastructure.ExternalServices
{
    public class MercadoPagoService : IPaymentMercadoPago
    {
        private readonly HttpClient _httpClient;

        public MercadoPagoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CheckoutResponse> CreateCheckoutAsync(MPCheckoutPayload payload)
        {
            var response = await _httpClient.PostAsJsonAsync("/mp/payment", payload);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<CheckoutResponse>();
            return result!;
        }
    }
}
