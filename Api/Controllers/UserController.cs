using Contracts.User.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Service;
using Contracts.User.Response;
using Domain.Entities;
using System.Security.Claims;


namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

   [Authorize(Roles = $"{nameof(TypeRole.SysAdmin)},{nameof(TypeRole.Admin)}")]
    [HttpGet]
    public ActionResult<List<UserResponse>> GetAllUsers()
    {
        var usersList = _userService.GetAll();
        if (!usersList.Any())
        {
            return NotFound();
        }
        return Ok(usersList);
    }
    [Authorize]
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var tokenUserId = User.FindFirst("userId")?.Value;
        if (role != nameof(TypeRole.SysAdmin) && role != nameof(TypeRole.Admin) && tokenUserId != id.ToString())
        {
            return StatusCode(403,"No tenes permisos para ver otros usuarios");
        }
            var user = _userService.GetById(id);

        return Ok(user);
    }

    [HttpPost]

    public ActionResult CreateUser([FromBody] CreateUserRequest user)
    {
        var isCreated = _userService.Create(user);
        if (!isCreated)
        {
            return BadRequest("No se pudo crear el usuario");
        }
        return Ok("Usuario creado");
    }


    //[HttpPatch("{id}/status")]
    //public ActionResult UpdateUserStatus([FromRoute] int id, [FromBody] ParcialUpdateUserRequest user)
    //{
    //    user.Id = id;
    //    var isActive = _userService.UpdateUserStatus(user);
    //    if (!isActive)
    //    {
    //        return Conflict("No se puede dar de baja al usuario");
    //    }
    //    return NoContent();
    //}

    [HttpPatch("{id}")]
    public ActionResult ParcialUpdateUser([FromRoute] int id, [FromBody] ParcialUpdateUserRequest user)
    {
        var isParcialUpdated = _userService.ParcialUpdateUser(id, user);

        if (!isParcialUpdated)
        {
            return Conflict("No se pudo actualizar el usuario parcialmente");
        }

        return NoContent();
    }

    [HttpPut("{id}")]
    public ActionResult UpdateUser([FromRoute] int id, [FromBody] UpdateUserRequest user)
    {
        var isUpdated = _userService.Update(id, user);

        if (!isUpdated)
        {
            return Conflict("Usuario no se pudo actualizar");
        }

        return NoContent();
    }

    [HttpDelete("{id}/soft")]
    public ActionResult SoftDeleteUser(int id)
    {
        var request = new SoftDeleteUserRequest { Id = id, IsDeleted = false };
        var result = _userService.SoftDeleteUser(id, request);
        if (!result)
        {
            return NotFound("Error al eliminar el usuario");
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteUser([FromRoute] int id)
    {
        var isDeleted = _userService.Delete(id);
        if (!isDeleted)
        {
            return BadRequest("Error al eliminar el usuario");
        }

        return NoContent();
    }
}
