using Newtonsoft.Json;
using PeliculasWeb.Repositorio.IRepositorio;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PeliculasWeb.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        //Injección de dependencias se debe importar el IHttpClientFactory
        private readonly IHttpClientFactory _clientFactory;

        public Repositorio(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<bool> ActualizarAsync(string url, T itemActualizar, string token = "")
        {
            var peticion = new HttpRequestMessage(HttpMethod.Patch, url);

            if (itemActualizar != null)
            {
                peticion.Content = new StringContent(
                    JsonConvert.SerializeObject(itemActualizar), Encoding.UTF8, "application/json"
                    );
            }
            else
            {
                return false;
            }

            var cliente = _clientFactory.CreateClient();

            //Aquí valida token
            if (token != null && token.Length != 0)
            {
                cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            //Validar si se actualizo y retorna boleano
            if (respuesta.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> BorrarAsync(string url, int Id, string token = "")
        {
            var peticion = new HttpRequestMessage(HttpMethod.Delete, url + Id);

            var cliente = _clientFactory.CreateClient();

            //Aquí valida token
            if (token != null && token.Length != 0)
            {
                cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            //Validar si se actualizo y retorna boleano
            if (respuesta.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<bool> CrearAsync(string url, T itemCrear, string token = "")
        {
            var peticion = new HttpRequestMessage(HttpMethod.Post, url);

            if (itemCrear != null)
            {
                peticion.Content = new StringContent(
                    JsonConvert.SerializeObject(itemCrear), Encoding.UTF8, "application/json"
                    );
            }
            else
            {
                return false;
            }

            var cliente = _clientFactory.CreateClient();

            //Aquí valida token
            if (token != null && token.Length != 0)
            {
                cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            //Validar si se actualizo y retorna boleano
            if (respuesta.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<T> GetAsync(string url, int Id)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url + Id);

            var cliente = _clientFactory.CreateClient();

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            //Validar si se actualizo y retorna los datos
            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable> GetTodoAsync(string url)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url);

            var cliente = _clientFactory.CreateClient();

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            //Validar si se actualizo y retorna los datos
            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable> GetPeliculasEnCategoriaAsync(string url, int categoriaId)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url + categoriaId);

            var cliente = _clientFactory.CreateClient();

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            //Validar si se encontraron y retorna los datos
            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable> Buscar(string url, string nombre)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url + nombre);

            var cliente = _clientFactory.CreateClient();

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            //Validar si se encontraron y retorna los datos
            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            else
            {
                return null;
            }
        }
    }
}
