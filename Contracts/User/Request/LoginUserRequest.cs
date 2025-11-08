using System.ComponentModel.DataAnnotations;

namespace Contracts.User.Request;

public class LoginUserRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Contraseña requerida")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)\S{8,15}$",
    ErrorMessage = "La contraseña debe tener minúsculas, mayúsculas, números y no contener espacios.")]
    public string Password { get; set; } = string.Empty;
}
