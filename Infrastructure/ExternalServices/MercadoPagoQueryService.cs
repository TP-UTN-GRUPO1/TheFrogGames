using Application.Abstraction.ExternalServices;
using Contracts.MercadoPago.Response;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class MercadoPagoQueryService : IMercadoPagoQueryService
{
    private readonly HttpClient _httpClient;

    public MercadoPagoQueryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PaymentDetailResponse> GetPaymentAsync(string paymentId)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", "TU_ACCESS_TOKEN_MP");

        var response = await _httpClient.GetAsync(
            $"https://api.mercadopago.com/v1/payments/{paymentId}");

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<PaymentDetailResponse>();
    }
}
