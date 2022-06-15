using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace PeliculasWeb.Models.ViewModels
{
    public class PeliculasVM
    {
        public IEnumerable<SelectListItem> ListaCategorias { get; set; }
        public Pelicula Pelicula { get; set; }
    }
}
