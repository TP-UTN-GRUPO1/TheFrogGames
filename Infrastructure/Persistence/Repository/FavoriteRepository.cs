using Application.Abstraction;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Persistence.Repository;

public class FavoritesRepository : IFavoriteRepository
{
    private readonly TheFrogGamesDbContext _context;

    public FavoritesRepository(TheFrogGamesDbContext context)
    {
        _context = context;
    }

    public async Task<List<Favorite>> GetByUser(int userId)
    {
        return await _context.Favorites
            .Include(f => f.Game)
            .Where(f => f.UserId == userId)
            .ToListAsync();
    }

    public async Task<bool> Exists(int userId, int gameId)
    {
        return await _context.Favorites
            .AnyAsync(f => f.UserId == userId && f.GameId == gameId);
    }

    public async Task Create(Favorite entity)
    {
        await _context.Favorites.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<Favorite?> Get(int userId, int gameId)
    {
        return await _context.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.GameId == gameId);
    }

    public async Task Delete(Favorite entity)
    {
        _context.Favorites.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
