using Contracts.Game.Request;
using Contracts.Game.Response;
using Domain.Entities;

namespace Application.Abstraction;

public interface IGameService
{
    List<GameResponse> GetAll();
    List<GameResponse> Search(string? name);
    List<GameResponse> GetByValue(decimal valor);
    GameResponse? GetGameById(int id);
    bool Create(CreateGameRequest request);
    bool Update(int id, CreateGameRequest request);
    bool UpdateKeyMetadata(int id, UpdateKeyMetadataGameRequest producto);
    bool Delete(int id);
    bool softDeleteGame(int id ,ParcialUpdateGameRequest request);
    Task AddGamesAsync(IEnumerable<GameResponse> games, CancellationToken cancellationToken = default);
     Task<IEnumerable<GameResponse>> GetAllAsync(CancellationToken cancellationToken);

    /* el cancellationtoken se usa para buenas practicas cuando hacemos cosas asincronicas , para que EF no siga
     ejecutando la consulta si ya cerre el http , es opcional segun la ia y el resto de internet se puede usar o no 
    en proyectos chicos no pasa nada peroo es buena practica
     */
}
