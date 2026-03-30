using Microsoft.EntityFrameworkCore;
using Application.Abstraction;
using Domain.Entities;

namespace Infrastructure.Persistence.Repository
{
    public class GameRepository : BaseRepository<Game>, IGameRepository
    {
        private readonly TheFrogGamesDbContext _context;

        public GameRepository(TheFrogGamesDbContext context) : base(context)
        {
            _context = context;
        }

        public TheFrogGamesDbContext Context => _context;
        public List<Game> GetAll() // como esta en infra devolvemos la entidad de dominio y no el dto ojo con eso
        {
            return _context.Games
                .Include(g => g.Genres)
                .Include(g => g.Platforms)
                .AsNoTrackingWithIdentityResolution()
                .ToList();
        }

        public Game? GetByIdWithRelations(int id, bool trackChanges = false)
        {
            var query = _context.Games
                .Include(g => g.Genres)
                .Include(g => g.Platforms)
                .AsQueryable();

            if (!trackChanges)
                query = query.AsNoTracking();

            return query.FirstOrDefault(g => g.Id == id);
        }

        public async Task<Game?> GetByIdAsync(int id)
        {
            return await _context.Games.FirstOrDefaultAsync(g => g.Id == id);
        }

        public Dictionary<int, decimal> GetPricesByIds(List<int> gameIds)
        {
            var prices = _context.Games 
                .Where(g => gameIds.Contains(g.Id))
                .Select(g => new { g.Id, g.Price }) 
                .ToDictionary(g => g.Id, g => g.Price);

            return prices;
        }
        public async Task AddAsync(Game game)
        {
            await _context.Games.AddAsync(game);
        }
        public async Task AddRangeAsync(IEnumerable<Game> games, CancellationToken ct = default)
        {
            await _context.Games.AddRangeAsync(games, ct);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public bool Create(Game game)
        {
            foreach (var genre in game.Genres)
            {
                if (genre.Id == 0)
                {
                    _context.Genres.Add(genre);
                }
                else
                {
                    _context.Attach(genre);
                }
            }

            foreach (var platform in game.Platforms)
            {
                if (platform.Id == 0)
                {
                    _context.Platforms.Add(platform);
                }
                else
                {
                    _context.Attach(platform);
                }
            }

            _context.Games.Add(game);
            return _context.SaveChanges() > 0;
        }
        public async Task<IEnumerable<Game>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Games
                .Include(g => g.Genres)
                .Include(g => g.Platforms)
                .ToListAsync(cancellationToken);
        }

    }
}
