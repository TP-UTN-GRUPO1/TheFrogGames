using Application.Service;
using Contracts.Favorites.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FavoritesController : ControllerBase
{
    private int GetUserIdFromToken()
    {
        var claim = User.FindFirst("idUser");

        if (claim == null)
            throw new Exception("Token inválido");

        return int.Parse(claim.Value);
    }

    private readonly IFavoriteService _favoritesService;
    public FavoritesController(IFavoriteService favoritesService)
    {
        _favoritesService = favoritesService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var userId = int.Parse(User.FindFirst("idUser")!.Value);

        var favorites = await _favoritesService.GetByUser(userId);

        return Ok(favorites);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddFavorite(int gameId)
    {
        var userId = GetUserIdFromToken();

        var request = new CreateFavoriteRequest
        {
            GameId = gameId
        };

        await _favoritesService.Create(request, userId);

        return Ok();
    }

    [Authorize]
    [HttpDelete("{gameId}")]
    public async Task<IActionResult> DeleteFavorite(int gameId)
    {
        var userId = GetUserIdFromToken();

        var result = await _favoritesService.DeleteFavorite(userId, gameId);

        if (!result)
            return BadRequest("No se encontró el favorito");

        return NoContent();
    }

}


