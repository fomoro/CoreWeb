using PeliculasWeb.Models;
using System.Threading.Tasks;

namespace PeliculasWeb.Repositorio.IRepositorio
{
    public interface IAccountRepositorio : IRepositorio<UsuarioM>
    {
        Task<UsuarioM> LoginAsync(string url, UsuarioM itemCrear);
        Task<bool> RegisterAsync(string url, UsuarioM itemCrear);
    }
}
