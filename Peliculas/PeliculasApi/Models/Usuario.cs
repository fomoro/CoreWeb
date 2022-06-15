using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeliculasApi.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        public string UsuarioA { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
