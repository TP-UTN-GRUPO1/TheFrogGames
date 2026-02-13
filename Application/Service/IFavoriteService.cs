using Contracts.Favorites.Request;
using Contracts.Favorites.Response;

namespace Application.Service;

public interface IFavoriteService
{
    Task<List<FavoriteResponse>> GetByUser(int userId);

    Task Create(CreateFavoriteRequest request, int userId);

    Task<bool> DeleteFavorite(int userId, int gameId);
}
