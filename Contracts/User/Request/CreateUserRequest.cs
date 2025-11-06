using System.ComponentModel.DataAnnotations;

namespace Contracts.User.Request;
public class CreateUserRequest
{
    [Required(ErrorMessage = "El nombre es requerido")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es requerido")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es requerido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
    public DateOnly BirthDate { get; set; }
    [Required(ErrorMessage = "Contraseña requerida")]
    public string Password { get; set; } = string.Empty;

}
