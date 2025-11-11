using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using Application.Abstraction.ExternalServices;
using Contracts.Game.Response;
using Infrastructure.Options;

namespace Infrastructure.ExternalServices
{
    public class GamesFromFirebaseService : IExternalGameService
    {
        private readonly HttpClient _httpClient;

        public GamesFromFirebaseService(HttpClient httpClient, IOptions<GamesApiOptions> options)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
                 _httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        
        public async Task<IEnumerable<GameResponse>> GetGames(CancellationToken cancellationToken = default)
        {
            try
            {
                Console.WriteLine("Obteniendo juegos desde firebase");
                var response = await _httpClient.GetFromJsonAsync<List<GameResponse>>(
                    "dataGames.json", cancellationToken);
                
                Console.WriteLine("jeugos obtenidos");
                return response ?? Enumerable.Empty<GameResponse>();
            }
            catch 
            {
                Console.WriteLine("Error al obtener los juegos");
                throw;
            }
        }
    }
}
