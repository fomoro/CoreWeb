using PeliculasApi.Models;
using System.Collections.Generic;

namespace PeliculasApi.Repository.IRepository
{
    public interface IUsuarioRepository
    {
        ICollection<Usuario> GetUsuarios();
        Usuario GetUsuario(int UsuarioId);
        bool ExisteUsuario(string usuario);
        Usuario Registro(Usuario usuario, string password);
        Usuario Login(string usuario, string password);
        bool Guardar();
    }
}
