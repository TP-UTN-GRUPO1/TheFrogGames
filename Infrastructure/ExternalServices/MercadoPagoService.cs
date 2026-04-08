using Application.Abstraction.ExternalServices;
using Contracts.MercadoPago.Request;
using Contracts.MercadoPago.Response;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Infrastructure.ExternalServices
{
    public class MercadoPagoService : IMercadoPagoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _accessToken;

        public MercadoPagoService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _accessToken = config["MercadoPago:AccessToken"]!;
        }

        public async Task<CheckoutResponse> CreateCheckoutAsync(MPCheckoutPayload payload)
        {
            var mpRequest = new
            {
                items = payload.Items.Select(item => new
                {
                    title = item.Title,
                    quantity = item.Quantity,
                    unit_price = item.UnitPrice,
                    currency_id = "ARS"
                }),
                payer = new
                {
                    email = payload.PayerEmail
                },
                external_reference = payload.OrderId.ToString(),
                binary_mode = true
            };


            var httpRequest = new HttpRequestMessage(
                HttpMethod.Post,
                "checkout/preferences"
            );

            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _accessToken);

            httpRequest.Content = JsonContent.Create(mpRequest);

            var response = await _httpClient.SendAsync(httpRequest);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<MercadoPagoResponse>();

            return new CheckoutResponse
            {
                CheckoutUrl = json!.init_point,
                PreferenceId = json.id
            };
        }
    }

    // DTO interno para mapear respuesta de MP
    public class MercadoPagoResponse
    {
        public string id { get; set; } = default!;
        public string init_point { get; set; } = default!;
    }
}