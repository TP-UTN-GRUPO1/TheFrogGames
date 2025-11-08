using Application.Service;
using Contracts.User.Request;
using Contracts.User.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        var tokenUserId = User.FindFirst("idUser")?.Value;
        if (role != nameof(TypeRole.SysAdmin) && role != nameof(TypeRole.Admin) && tokenUserId != id.ToString())
        {
            return StatusCode(403, "No tenes permisos para ver otros usuarios");
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

    [Authorize]
    [HttpPatch("{id}")]
    public ActionResult CompleteUserInfo([FromRoute] int id, [FromBody] CompleteUserInfoRequest user)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var tokenUserId = User.FindFirst("idUser")?.Value;
        if (role != nameof(TypeRole.SysAdmin) && role == nameof(TypeRole.Admin) && tokenUserId != id.ToString())
        {
            return StatusCode(403, "No tenes permisos para editar otros usuarios");
        }
        var isUpdated = _userService.CompleteUserInfo(id, user);

        if (!isUpdated)
        {
            return Conflict("No se pudo actualizar el usuario o usuario inexistente");
        }

        return NoContent();
    }


    [Authorize(Roles = $"{nameof(TypeRole.SysAdmin)}")]
    [HttpDelete("{id}/soft")]
    public ActionResult SoftDeleteUser(int id)
    {
        var request = new SoftDeleteUserRequest { Id = id, IsDeleted = true };
        var result = _userService.SoftDeleteUser(id, request);
        if (!result)
        {
            return NotFound("Error al eliminar el usuario");
        }
        return NoContent();
    }
    [Authorize(Roles =$"{nameof(TypeRole.SysAdmin)}")]
    [HttpPut("{id}/role")]
    public ActionResult ChangeRole([FromRoute] int id, [FromBody] ChangeRoleRequest request)
    {
        var isRoleChanged = _userService.ChangeRole(id, request);
        if (!isRoleChanged)
        {
            return Conflict("No se pudo actualizar el rol");
        }
        return NoContent();
    } 
}
