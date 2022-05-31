using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.Models.Dtos
{
    public class UsuarioAuthLoginDto
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string Password { get; set; }
    }
}
