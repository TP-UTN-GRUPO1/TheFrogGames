using Contracts.Game.Response;
using Domain.Entities;
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
            var response = await _httpClient.GetFromJsonAsync<List<GameResponse>>(
                "dataGames.json", ct);

            return response ?? Enumerable.Empty<GameResponse>();
        }
    }
}