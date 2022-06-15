﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeliculasApi.Models
{
    public class Pelicula
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public byte[] RutaImagen { get; set; }
        public string Descripcion { get; set; }
        public string Duracion { get; set; }
        public enum TipoClasificacion { Siete, Trece, Dieciseis, Dieciocho }
        public TipoClasificacion Clasificacion { get; set; }
        public DateTime FechaCreacion { get; set; }

        public int categoriaId { get; set; }
        [ForeignKey("categoriaId")]
        public Categoria Categoria { get; set; }
    }
}
