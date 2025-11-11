using System.ComponentModel.DataAnnotations;

namespace Contracts.Game.Request
{
    public class CreateGameRequest
    {
        [Required(ErrorMessage = "Falta colocar un titulo")]
        [MaxLength(100, ErrorMessage = "El título no puede superar los 100 caracteres")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "El desarrollador es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre del desarrollador no puede superar los 100 caracteres")]
        public string Developer { get; set; } = string.Empty;

        [Required(ErrorMessage = "La URL img es obligatoria")]
        [Url(ErrorMessage = "Debe ingresar una URL válida.")]
        public string ImageUrl { get; set; } = string.Empty;

        [Range(0.01, 10000, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Price { get; set; }

        public bool Available { get; set; } = true;

        [Range(0, 10, ErrorMessage = "El rating debe estar entre 0 y 10")]
        public float Rating { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El número de vendidos no puede ser negativo")]
        public int Sold { get; set; }

        [MinLength(1, ErrorMessage = "Debe incluir una o mas plataforma")]
        public List<string> Platforms { get; set; } = new();

        [MinLength(1, ErrorMessage = "Debe incluir uno o mas generos")]
        public List<string> Genres { get; set; } = new();
    }
}
