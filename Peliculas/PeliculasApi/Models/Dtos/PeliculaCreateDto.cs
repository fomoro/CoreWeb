using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using static PeliculasApi.Models.Pelicula;

namespace PeliculasApi.Models.Dtos
{
    public class PeliculaCreateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }
        public string RutaImagen { get; set; }
        public IFormFile Foto { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Duracion { get; set; }
        public TipoClasificacion Clasificacion { get; set; }

        public int categoriaId { get; set; }
    }
}
