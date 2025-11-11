using Contracts.Game.Response;
using System.Net.Http.Json;

namespace Infrastructure.Http
{
    public class GamesFirebaseClient
    {
        private readonly HttpClient _httpClient;

        public GamesFirebaseClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<GameResponse>> GetGamesAsync(CancellationToken ct = default)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<GameResponse>>(
                    "dataGames.json", ct);

                return response ?? Enumerable.Empty<GameResponse>();
            }
            catch (Exception ex)
            {
                // Polly ya reintentará, pero logueamos para debug
                Console.WriteLine($"[Firebase] Error al obtener juegos: {ex.Message}");
                return Enumerable.Empty<GameResponse>();
            }
        }
    }
}