using Application.Abstraction;
using Application.Service;
using Contracts.Game.Request;
using Contracts.Game.Response;
using Contracts.User.Request;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }
      
        [HttpGet]
        public async Task<ActionResult<List<GameResponse>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var listGame = await _gameService.GetAllAsync(cancellationToken);

            if (listGame == null || !listGame.Any())
            {
                return NotFound("No se encontraron juegos en la base de datos.");
            }

            return Ok(listGame);
        }
        [Authorize(Roles = $"{nameof(TypeRole.SysAdmin)},{nameof(TypeRole.Admin)}")]
        [HttpPost]
        public ActionResult Create([FromBody] CreateGameRequest game)
        {
            var isCreated = _gameService.Create(game);

            if (!isCreated)
            {
                return Conflict("Error al crear el producto");
            }

            return CreatedAtAction(nameof(GetById), new { id = game.Id }, game.Id);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var game = _gameService.GetGameById(id);
            if (game == null) return NotFound();
            return Ok(game);
        }
        [Authorize(Roles = $"{nameof(TypeRole.SysAdmin)},{nameof(TypeRole.Admin)}")]
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] CreateGameRequest game)
        {
            var isUpdated = _gameService.Update(id, game);
            if (!isUpdated)
            {
                return Conflict("No se pudo actualizar el juego");
            }
            return NoContent();
        }
        [Authorize(Roles = $"{nameof(TypeRole.SysAdmin)},{nameof(TypeRole.Admin)}")]
        [HttpPatch("{id}/notavailable")]
        public IActionResult SoftDelete(int id)
        {
            var request = new ParcialUpdateGameRequest { Id = id, Available = false };
            var result = _gameService.softDeleteGame(id, request);

            if (!result)
                return NotFound();

            return NoContent();
        }
        [Authorize(Roles = $"{nameof(TypeRole.SysAdmin)},{nameof(TypeRole.Admin)}")]
        [HttpPatch("{id}/available")]
        public IActionResult Restore(int id)
        {
            var request = new ParcialUpdateGameRequest { Id = id, Available = true };
            var result = _gameService.softDeleteGame(id, request);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string? name)
        {
            var result = _gameService.Search(name);
            return Ok(result);
        }
    }
}