using Contracts.Game.Response;

namespace Application.Abstraction.ExternalServices
{
    public interface IExternalGameService
    {
        Task<IEnumerable<GameResponse>> GetGames(CancellationToken cancellationToken = default);
    }
}
