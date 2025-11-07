using Application.Abstraction;
using Domain.Entities;

namespace Infrastructure.Persistence.Repository
{
    public class PlatformRepository : BaseRepository<Platform>, IPlatformRepository
    {
        public PlatformRepository(TheFrogGamesDbContext context) : base(context)
        {
         

        }

        public async Task AddAsync(Platform platform)
        {
            await _context.Platforms.AddAsync(platform);

        }
    }


}