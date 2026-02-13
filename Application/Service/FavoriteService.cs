using Application.Abstraction;
using Contracts.Favorites.Request;
using Contracts.Favorites.Response;
using Domain.Entities;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Application.Service;

public class FavoriteService : IFavoriteService
{
    private readonly IFavoriteRepository _favoritesRepository;

    public FavoriteService(IFavoriteRepository favoritesRepository)
    {
        _favoritesRepository = favoritesRepository;
    }
    public async Task Create(CreateFavoriteRequest request, int userId)
    {
        var exists = await _favoritesRepository.Exists(userId, request.GameId);

        if (exists)
            throw new Exception("El juego ya está en favoritos");

        var favorite = new Favorite
        {
            UserId = userId,
            GameId = request.GameId
        };

        await _favoritesRepository.Create(favorite);
    }


    public async Task<List<FavoriteResponse>> GetByUser(int userId)
    {
        var favorites = await _favoritesRepository.GetByUser(userId);

        return favorites.Select(f => new FavoriteResponse
        {
            GameId = f.GameId,
            Title = f.Game.Title,
            ImageUrl = f.Game.ImageUrl,
            Price = f.Game.Price
        }).ToList();
    }


    public async Task<bool> DeleteFavorite(int userId, int gameId)
    {
        var favorite = await _favoritesRepository.Get(userId, gameId);

        if (favorite == null)
            return false;

        await _favoritesRepository.Delete(favorite);
        return true;
    }
}