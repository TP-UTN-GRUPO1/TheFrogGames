using System.ComponentModel.DataAnnotations;

namespace Contracts.User.Request;

public class LoginUserRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Contraseña requerida")]
    public string Password { get; set; } = string.Empty;
}
