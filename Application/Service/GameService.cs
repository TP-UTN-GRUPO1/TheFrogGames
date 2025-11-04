using Application.Abstraction;
using Contracts.Game.Request;
using Contracts.Game.Response;
using Domain.Entities;

namespace Application.Service
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepo;
        private readonly IGenreRepository _genreRepo;
        private readonly IPlatformRepository _platformRepo;

        public GameService(IGameRepository gameRepo,
            IPlatformRepository platformRepo,
            IGenreRepository genreRepo)
        {
            _gameRepo = gameRepo;
            _platformRepo =platformRepo;
            _genreRepo = genreRepo;
        }

        public List<GameResponse> GetAll()
        {
            var games = _gameRepo.GetAll();

            return games.Select(g => new GameResponse
            {
                Id = g.Id,
                Title = g.Title,
                Price = g.Price,
                Developer = g.Developer,
                ImageUrl = g.ImageUrl,
                Rating = g.Rating,
                Available = g.Available,
                Sold = g.Sold,
                Platforms = g.Platforms.Select(p => p.Name).ToList(),
                Genres = g.Genres.Select(ge => ge.Name).ToList()
            }).ToList();
        }

        public GameResponse? GetGameById(int id)
        {
            var game = _gameRepo.GetById(id);
            if (game == null) return null;

            return new GameResponse
            {
                Id = game.Id,
                Title = game.Title,
                Price = game.Price,
                Developer = game.Developer,
                ImageUrl = game.ImageUrl,
                Rating = game.Rating,
                Available = game.Available,
                Sold = game.Sold
            };
        }

        public List<GameResponse> Search(string? name)
        {
            var games = _gameRepo.GetAll().AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                games = games.Where(g => g.Title.Contains(name, StringComparison.OrdinalIgnoreCase));

            return games.Select(g => new GameResponse
            {
                Id = g.Id,
                Title = g.Title,
                Price = g.Price,
                Developer = g.Developer,
                ImageUrl = g.ImageUrl,
                Rating = g.Rating,
                Available = g.Available,
                Sold = g.Sold
            }).ToList();
        }

        public List<GameResponse> GetByValue(decimal valor)
        {
            var games = _gameRepo.GetAll().Where(g => g.Price == valor);

            return games.Select(g => new GameResponse
            {
                Id = g.Id,
                Title = g.Title,
                Price = g.Price,
                Developer = g.Developer,
                ImageUrl = g.ImageUrl,
                Rating = g.Rating,
                Available = g.Available,
                Sold = g.Sold,
                Platforms = g.Platforms.Select(p => p.Name).ToList(),
                Genres = g.Genres.Select(g => g.Name).ToList()
            }).ToList();
        }


        public bool Create(CreateGameRequest request)
        {

            var existingGenres = _genreRepo.GetAll().ToList();
            var existingPlatforms = _platformRepo.GetAll().ToList();


            var game = new Game
            {
                Title = request.Title,
                Price = request.Price,
                Developer = request.Developer,
                ImageUrl = request.ImageUrl,
                Rating = request.Rating,
                Available = request.Available,
                Sold = request.Sold,
                Genres = new List<Genre>(),
                Platforms = new List<Platform>()
            };
            foreach (var genreName in request.Genres.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                var existingGenre = existingGenres
                    .FirstOrDefault(g => g.Name.Equals(genreName, StringComparison.OrdinalIgnoreCase));

                if (existingGenre != null)
                {
                    
                    game.Genres.Add(existingGenre);
                }
                else
                {
                    
                    var newGenre = new Genre { Name = genreName };
                    game.Genres.Add(newGenre);
                }
            }
            foreach (var platformName in request.Platforms.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                var existingPlatform = existingPlatforms
                    .FirstOrDefault(p => p.Name.Equals(platformName, StringComparison.OrdinalIgnoreCase));

                if (existingPlatform != null)
                {
                    game.Platforms.Add(existingPlatform);
                }
                else
                {
                    var newPlatform = new Platform { Name = platformName };
                    game.Platforms.Add(newPlatform);
                }
            }
            bool success = _gameRepo.Create(game);

            if (!success)
                throw new ApplicationException("Error al crear el juego en la base de datos.");

            return true;
        }

        public bool Update(int id, CreateGameRequest request)
        {
            var existingGame = _gameRepo.GetById(id, trackChanges: true);
            if (existingGame == null)
                throw new Exception($"Juego con ID {id} no encontrado.");

            existingGame.Title = request.Title;
            existingGame.Price = request.Price;
            existingGame.Developer = request.Developer;
            existingGame.ImageUrl = request.ImageUrl;
            existingGame.Rating = request.Rating;
            existingGame.Available = request.Available;
            existingGame.Sold = request.Sold;

            bool success = _gameRepo.Update(existingGame);

            if (!success)
                throw new ApplicationException("Error al actualizar el juego en la base de datos.");

            return true;
        }

        public bool UpdateKeyMetadata(int id, UpdateKeyMetadataGameRequest producto)
        {
            var existingGame = _gameRepo.GetById(id, trackChanges: true);
            if (existingGame == null)
                throw new Exception($"Juego con ID {id} no encontrado.");

         

            bool success = _gameRepo.Update(existingGame);

            if (!success)
                throw new ApplicationException("Error al actualizar la metadata del juego.");

            return true;
        }

        public bool Delete(int id)
        {
            var gameToDelete = _gameRepo.GetById(id);
            if (gameToDelete == null)
                return true; // nada que borrar

            return _gameRepo.Delete(gameToDelete);
        }

        public bool softDeleteGame(int id, ParcialUpdateGameRequest request)
        {
            var gameToDelete = _gameRepo.GetById(id, trackChanges: true);
            if (gameToDelete == null)
                throw new Exception($"Juego con ID {id} no encontrado.");
            gameToDelete.Available = request.Available;

            bool success = _gameRepo.Update(gameToDelete);

            if(!success)
                throw new ApplicationException("Error al actualizar la disponibilidad del juego.");
            return true;
        }

        public Task AddGamesAsync(IEnumerable<GameResponse> games, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<GameResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            var games = await _gameRepo.GetAllAsync(cancellationToken);

            return games.Select(g => new GameResponse
            {
                Id = g.Id,
                Title = g.Title,
                Developer = g.Developer,
                ImageUrl = g.ImageUrl,
                Price = g.Price,
                Available = g.Available,
                Rating = g.Rating,
                Sold = g.Sold,
                Genres = g.Genres.Select(ge => ge.Name).ToList(),
                Platforms = g.Platforms.Select(p => p.Name).ToList()
            });
        }
    }
}
