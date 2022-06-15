using PeliculasWeb.Models;
using PeliculasWeb.Repositorio.IRepositorio;
using System.Net.Http;

namespace PeliculasWeb.Repositorio
{
    public class UsuarioRepositorio : Repositorio<UsuarioU>, IUsuarioRepositorio
    {
        //Injección de dependencias se debe importar el IHttpClientFactory
        private readonly IHttpClientFactory _clientFactory;

        public UsuarioRepositorio(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

    }
}
