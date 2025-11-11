
using Application.Abstraction.ExternalServices;
using Contracts.Game.Response;
using Infrastructure.Http;

namespace Infrastructure.ExternalServices
{
    public class GamesFromFirebaseService : IExternalGameService
    {
        private readonly GamesFirebaseClient _client;

        public GamesFromFirebaseService(GamesFirebaseClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<GameResponse>> GetGames(CancellationToken ct = default)
        {
            return await _client.GetGamesAsync(ct);
        }
    }
}