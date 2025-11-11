using Contracts.Game.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
    
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
                Console.WriteLine($"[Firebase] Error: {ex.Message}");
                return Enumerable.Empty<GameResponse>();
            }
        }
    }
}
