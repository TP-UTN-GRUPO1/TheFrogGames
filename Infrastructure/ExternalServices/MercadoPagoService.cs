using Application.Abstraction.ExternalServices;
using Contracts.MercadoPago.Request;
using Contracts.MercadoPago.Response;
using System.Net.Http.Json;

namespace Infrastructure.ExternalServices
{
    public class MercadoPagoService : IMercadoPagoService
    {
        private readonly HttpClient _httpClient;

        public MercadoPagoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            Console.WriteLine("BASE ADDRESS: " + _httpClient.BaseAddress);
        }

        public async Task<CheckoutResponse> CreatePreferenceAsync(
            CreatePreferenceRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/mp/payment",
                request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<CheckoutResponse>();

            return result!;
        }
    }
}