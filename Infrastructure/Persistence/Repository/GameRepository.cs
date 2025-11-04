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
        public List<Game> GetAll() // como esta en infra devolvemos la entidad de dominio y no el dto ojo con eso
        {
            return _context.Games
                .Include(g => g.Genres)
                .Include(g => g.Platforms)
                .AsNoTrackingWithIdentityResolution()
                .ToList();
        }
        public async Task<Game?> GetByIdAsync(int id)
        {
            return await _context.Games.FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task AddAsync(Game game)
        {
            await _context.Games.AddAsync(game);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public bool Create(Game game)
        {
            
            _context.AttachRange(game.Genres.Where(g => g.Id != 0));
            _context.AttachRange(game.Platforms.Where(p => p.Id != 0));

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
