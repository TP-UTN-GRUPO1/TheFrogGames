using Application.Abstraction.ExternalServices;
using Domain.Entities;
using Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Seeding
{
    public class GamesSeeder
    {
        private readonly IExternalGameService _externalGameService;
        private readonly GameRepository _gameRepository;

        public GamesSeeder(IExternalGameService externalGameService, GameRepository gameRepository)
        {
            _externalGameService = externalGameService;
            _gameRepository = gameRepository;
        }

        public async Task SeedAsync()
        {
            var existingGames = await _gameRepository.GetAllAsync();
            if (existingGames.Any())
            {
                Console.WriteLine("▬▬ Ya tenes cargados juegos en la BD ▬▬");
                return;
            }
            var games = await _externalGameService.GetGames();
            foreach (var game in games)
            {
                var entity = new Game
                {
                    Title = game.Title,
                    Developer = game.Developer,
                    Price = game.Price,
                    Rating = game.Rating,
                    ImageUrl = game.ImageUrl,
                    Available = game.Available

                };
                await _gameRepository.AddAsync(entity);
            }
            await _gameRepository.SaveChangesAsync();
            Console.WriteLine("▬▬ Se cargaron los juegos traidos desde Firebase ▬▬");
        }
    }
}
