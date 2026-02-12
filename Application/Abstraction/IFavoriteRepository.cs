using Contracts.Favorites.Response;
using Domain.Entities;

namespace Application.Abstraction;

public interface IFavoriteRepository
{
    Task<List<Favorite>> GetByUser(int userId);

    Task<bool> Exists(int userId, int gameId);

    Task Create(Favorite favorite);
}
