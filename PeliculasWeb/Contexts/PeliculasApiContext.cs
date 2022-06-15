namespace PeliculasWeb.Contexts
{
    public static class PeliculasApiContext
    {
        public static string UrlBaseApi = "https://localhost:44342/";
        public static string RutaCategoriasApi = UrlBaseApi + "api/Categorias/";
        public static string RutaPeliculasApi = UrlBaseApi + "api/Peliculas/";
        public static string RutaUsuariosApi = UrlBaseApi + "api/Usuarios/";
        
        public static string RutaPeliculasEnCategoriaApi = UrlBaseApi + "api/Peliculas/GetPeliculasEnCategoria/";
        public static string RutaPeliculasApiBusqueda = UrlBaseApi + "api/Peliculas/Buscar?nombre=";
    }
}
