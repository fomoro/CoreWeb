using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PeliculasWeb.Contexts;
using PeliculasWeb.Models;
using PeliculasWeb.Models.ViewModels;
using PeliculasWeb.Repositorio.IRepositorio;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasWeb.Controllers
{
    public class PeliculasController : Controller
    {
        private readonly ICategoriaRepositorio _repoCategoria;
        private readonly IPeliculaRepositorio _repoPelicula;

        public PeliculasController(ICategoriaRepositorio repoCategoria, IPeliculaRepositorio repoPelicula)
        {
            _repoCategoria = repoCategoria;
            _repoPelicula = repoPelicula;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new Pelicula() { });
        }

        [HttpGet]
        public async Task<IActionResult> GetTodasPeliculas()
        {
            return Json(new { data = await _repoPelicula.GetTodoAsync(PeliculasApiContext.RutaPeliculasApi) });
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            IEnumerable<Categoria> npList = (IEnumerable<Categoria>)await _repoCategoria.GetTodoAsync(PeliculasApiContext.RutaCategoriasApi);

            PeliculasVM objVM = new PeliculasVM()
            {
                ListaCategorias = npList.Select(i => new SelectListItem
                {
                    Text = i.Nombre,
                    Value = i.Id.ToString()
                }),

                Pelicula = new Pelicula()
            };

            return View(objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pelicula pelicula)
        {

            IEnumerable<Categoria> npList = (IEnumerable<Categoria>)await _repoCategoria.GetTodoAsync(PeliculasApiContext.RutaCategoriasApi);

            PeliculasVM objVM = new PeliculasVM()
            {
                ListaCategorias = npList.Select(i => new SelectListItem
                {
                    Text = i.Nombre,
                    Value = i.Id.ToString()
                }),

                Pelicula = new Pelicula()
            };

            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    pelicula.RutaImagen = p1;
                }
                else
                {
                    return View(objVM);
                }
                
                //string token = HttpContext.Session.GetString("JWToken");
                string token = null;
                await _repoPelicula.CrearAsync(PeliculasApiContext.RutaPeliculasApi, pelicula, token);
                return RedirectToAction(nameof(Index));
            }



            return View(objVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            IEnumerable<Categoria> npList = (IEnumerable<Categoria>)await _repoCategoria.GetTodoAsync(PeliculasApiContext.RutaCategoriasApi);

            PeliculasVM objVM = new PeliculasVM()
            {
                ListaCategorias = npList.Select(i => new SelectListItem
                {
                    Text = i.Nombre,
                    Value = i.Id.ToString()
                }),

                Pelicula = new Pelicula()
            };

            if (id == null)
            {
                return NotFound();
            }

            //Para mostrar los datos en el formulario Edit
            objVM.Pelicula = await _repoPelicula.GetAsync(PeliculasApiContext.RutaPeliculasApi, id.GetValueOrDefault());
            if (objVM.Pelicula == null)
            {
                return NotFound();
            }

            return View(objVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Pelicula pelicula)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    pelicula.RutaImagen = p1;
                }
                else
                {
                    var peliculaFromDb = await _repoPelicula.GetAsync(PeliculasApiContext.RutaPeliculasApi, pelicula.Id);
                    pelicula.RutaImagen = peliculaFromDb.RutaImagen;
                }

                //string token = HttpContext.Session.GetString("JWToken");
                string token = null;
                await _repoPelicula.ActualizarAsync(PeliculasApiContext.RutaPeliculasApi + pelicula.Id, pelicula, token);
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            //string token = HttpContext.Session.GetString("JWToken");
            string token = null;
            var status = await _repoPelicula.BorrarAsync(PeliculasApiContext.RutaPeliculasApi, id, token);

            if (status)
            {
                return Json(new { success = true, message = "Borrado correctamente" });
            }

            return Json(new { success = false, message = "No se pudo borrar" });
        }

        [HttpGet]
        public async Task<IActionResult> GetPeliculasEnCategoria(int id)
        {
            return Json(new { data = await _repoPelicula.GetPeliculasEnCategoriaAsync(PeliculasApiContext.RutaPeliculasEnCategoriaApi, id) });
        }
    }
}

