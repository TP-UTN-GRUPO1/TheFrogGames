using Microsoft.AspNetCore.Mvc;
using Contracts.Platform.Request;
using Application.Service;
using Contracts.Platform.Response;

namespace TheFrogGames.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformService _platformService;
        public PlatformController(IPlatformService platformService)
        {
            _platformService = platformService;
        }
        [HttpGet]
        public ActionResult<List<PlatformResponse>> GetAll()
        {
            var list = _platformService.GetPlatform();
            return Ok(list);
        }
        [HttpPost]
        public IActionResult Create([FromBody] CreatePlatformRequest request)
        {
            try
            {
                var newPlatform = _platformService.CreatePlatform(request);
                return CreatedAtAction(
                    nameof(GetAll),
                    new { id = newPlatform.Id },
                    newPlatform);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno del servidor: {ex.Message}");
            }
        }
        [HttpPut]
        public IActionResult Update([FromBody] UpdatePlatformRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedPlatform = _platformService.UpdatePlatform(request);
                return Ok(updatedPlatform);
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
                return BadRequest("El ID de la plataforma debe ser positivo.");
            }
            bool success = _platformService.DeletePlatform(id);

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