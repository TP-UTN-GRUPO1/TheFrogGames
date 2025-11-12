using Application.Abstraction;
using Contracts.Game.Request;
using Contracts.Game.Response;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

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
                Sold = game.Sold,
                Platforms = game.Platforms.Select(p => p.Name).ToList(),
                Genres = game.Genres.Select(ge => ge.Name).ToList()
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
                Sold = g.Sold,
                Platforms = g.Platforms.Select(p => p.Name).ToList(),
                Genres = g.Genres.Select(ge => ge.Name).ToList()
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
            /* que ningun campo este vacio */
            if (request == null)
                throw new ArgumentException("La solicitud de creación no puede ser nula.");

            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("El título del juego no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(request.Developer))
                throw new ArgumentException("El desarrollador no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(request.ImageUrl))
                throw new ArgumentException("La URL de la imagen no puede estar vacía.");

            if (request.Price <= 0)
                throw new ArgumentException("El precio debe ser mayor a 0.");

            if (request.Rating < 0 || request.Rating > 10)
                throw new ArgumentException("El rating debe estar entre 0 y 10.");

            if (request.Genres == null || !request.Genres.Any())
                throw new ArgumentException("Debe especificar al menos un género.");

            if (request.Platforms == null || !request.Platforms.Any())
                throw new ArgumentException("Debe especificar al menos una plataforma.");

            /* nos fijamos que ningun juego ya exista en la bd asi evitamos un duplicado
             el core de esto seria que el juego ya venga multiplataforma y no que sea uno por 
            plataforma , ej tenes gta 5 que esta para todo, la idea seria evitar un id x juego/platf*/
            var normalizedTitle = request.Title.Trim();

            var existingGame = _gameRepo
                .GetAll()
                .FirstOrDefault(g => g.Title.Equals(normalizedTitle, StringComparison.OrdinalIgnoreCase));

            if (existingGame != null)
                throw new InvalidOperationException($"Ya existe un juego con el título '{normalizedTitle}'.");

           
            var existingGenres = _genreRepo.GetAll().ToList();
            var existingPlatforms = _platformRepo.GetAll().ToList();

      
            var game = new Game
            {
                Title = normalizedTitle,
                Price = request.Price,
                Developer = request.Developer.Trim(),
                ImageUrl = request.ImageUrl.Trim(),
                Rating = request.Rating,
                Available = request.Available,
                Sold = request.Sold,
                Genres = new List<Genre>(),
                Platforms = new List<Platform>()
            };

           
            foreach (var genreName in request.Genres.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                var genre = existingGenres
                    .FirstOrDefault(g => g.Name.Equals(genreName, StringComparison.OrdinalIgnoreCase))
                    ?? new Genre { Name = genreName };
                game.Genres.Add(genre);
            }

            
            foreach (var platformName in request.Platforms.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                var platform = existingPlatforms
                    .FirstOrDefault(p => p.Name.Equals(platformName, StringComparison.OrdinalIgnoreCase))
                    ?? new Platform { Name = platformName };
                game.Platforms.Add(platform);
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

        public async Task AddGamesAsync(IEnumerable<GameResponse> games, CancellationToken ct = default)
        {
            if (games == null || !games.Any())
                throw new ArgumentException("La lista de juegos externos esta vacia.");

            var existingGames = await _gameRepo.GetAllAsync(ct);

            if (existingGames.Any())
                throw new InvalidOperationException("La base de datos ya contiene juegos.");

            var gameEntities = games.Select(g => new Game
            {
                Title = g.Title,
                Developer = g.Developer,
                ImageUrl = g.ImageUrl,
                Price = g.Price,
                Available = g.Available,
                Rating = g.Rating,
                Sold = g.Sold,
                Genres = g.Genres?.Select(name => new Genre { Name = name }).ToList() ?? new List<Genre>(),
                Platforms = g.Platforms?.Select(name => new Platform { Name = name }).ToList() ?? new List<Platform>()
            }).ToList();

            await _gameRepo.AddRangeAsync(gameEntities, ct);
            await _gameRepo.SaveChangesAsync();

            Console.WriteLine($" Se guardaron {gameEntities.Count} juegos en la BD.");
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
