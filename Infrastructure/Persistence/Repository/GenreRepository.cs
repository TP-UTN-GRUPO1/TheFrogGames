using Application.Abstraction;
using Domain.Entities;

namespace Infrastructure.Persistence.Repository
{
    public class GenreRepository : BaseRepository<Genre>, IGenreRepository
    {
        public GenreRepository(TheFrogGamesDbContext context) : base(context)
        {
        }
        public async Task AddAsync(Genre genre)
        {
            await _context.Genres.AddAsync(genre);

        }



    }
    

    }
