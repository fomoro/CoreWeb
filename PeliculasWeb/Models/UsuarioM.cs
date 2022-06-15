using System.ComponentModel.DataAnnotations;

namespace PeliculasWeb.Models
{
    public class UsuarioM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "La contraseña debe tener entre 4 y 10 caracteres")]
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
