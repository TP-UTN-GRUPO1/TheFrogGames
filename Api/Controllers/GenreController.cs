using Microsoft.AspNetCore.Mvc;
using Contracts.Genre.Request;
using Application.Service;
using Contracts.Genre.Response;
using Contracts.Order.Request;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;
        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }
        [HttpGet]
        public ActionResult<List<GenreResponse>> GetAll()
        {
            var list = _genreService.GetGenres();
            return Ok(list);
        }
        [HttpPost]
        public IActionResult Create([FromBody] CreateGenreRequest request)
        {
            try
            {
                var newGenre = _genreService.CreateGenre(request);
                return CreatedAtAction(
                    nameof(GetAll),
                    new { id = newGenre.Id },
                    newGenre);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno del servidor: {ex.Message}");
            }
        }
        [HttpPut]
        public IActionResult Update([FromBody] UpdateGenreRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedGenre = _genreService.UpdateGenre(request);
                return Ok(updatedGenre);
            }
            catch (Exception ex) when (ex.Message.Contains("no encontrado"))
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID del género debe ser positivo.");
            }
            bool success = _genreService.DeleteGenre(id);

            if (success)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al intentar eliminar el género.");
            }
        }
    }
}