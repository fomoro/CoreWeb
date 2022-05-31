using System;
using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        /// <summary>
        /// Fecha de Creacion
        /// </summary>
        public DateTime FechaCreacion { get; set; }
    }
}
