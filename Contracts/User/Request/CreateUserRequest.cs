using System.ComponentModel.DataAnnotations;

namespace Contracts.User.Request;
public class CreateUserRequest
{
    [Required(ErrorMessage = "El nombre es requerido")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es requerido")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage ="El formato del correo no es valido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
    public DateOnly BirthDate { get; set; }
    [Required(ErrorMessage = "Contraseña requerida")]
    [StringLength(20, MinimumLength =8, ErrorMessage ="La contraseña debe tener al menos 8 caracteres")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)\S{8,15}$",
    ErrorMessage = "La contraseña debe tener minúsculas, mayúsculas, números y no contener espacios.")]
    public string Password { get; set; } = string.Empty;

}
