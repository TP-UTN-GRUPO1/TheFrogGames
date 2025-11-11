using Application.Abstraction.ExternalServices;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repository;

namespace Infrastructure.Seeding
{
    public class GamesSeeder
    {
        private readonly IExternalGameService _externalGameService;
        private readonly GameRepository _gameRepository;
        private readonly TheFrogGamesDbContext _context;

        public GamesSeeder(
            IExternalGameService externalGameService,
            GameRepository gameRepository,
            TheFrogGamesDbContext context)
        {
            _externalGameService = externalGameService;
            _gameRepository = gameRepository;
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (_context.Games.Any())
            {
                Console.WriteLine("▬▬ Ya tenes cargados juegos en la BD ▬▬");
                return;
            }

            var games = await _externalGameService.GetGames();

          
            var existingGenres = _context.Genres.ToList();
            var existingPlatforms = _context.Platforms.ToList();

            foreach (var game in games)
            {
                var entity = new Game
                {
                    Title = game.Title,
                    Developer = game.Developer,
                    Price = game.Price,
                    Rating = game.Rating,
                    ImageUrl = game.ImageUrl,
                    Available = game.Available,
                    Genres = new List<Genre>(),
                    Platforms = new List<Platform>()
                };

                
                foreach (var genreName in game.Genres)
                {
                    var genre = existingGenres.FirstOrDefault(g => g.Name == genreName);
                    if (genre == null)
                    {
                        genre = new Genre { Name = genreName };
                        existingGenres.Add(genre);
                        _context.Genres.Add(genre);
                    }
                    entity.Genres.Add(genre);
                }

           
                foreach (var platformName in game.Platforms)
                {
                    var platform = existingPlatforms.FirstOrDefault(p => p.Name == platformName);
                    if (platform == null)
                    {
                        platform = new Platform { Name = platformName };
                        existingPlatforms.Add(platform);
                        _context.Platforms.Add(platform);
                    }
                    entity.Platforms.Add(platform);
                }

                _context.Games.Add(entity);
            }

            await _context.SaveChangesAsync();
            Console.WriteLine("Se cargaron los juegos en la bd con sus respectivos generos y plataformas");
        }
    }
}
