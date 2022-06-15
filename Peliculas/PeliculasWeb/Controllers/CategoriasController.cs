using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeliculasWeb.Contexts;
using PeliculasWeb.Models;
using PeliculasWeb.Repositorio.IRepositorio;
using System.Threading.Tasks;

namespace PeliculasWeb.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ICategoriaRepositorio _repoCategoria;

        public CategoriasController(ICategoriaRepositorio repoCategoria)
        {
            _repoCategoria = repoCategoria;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new Categoria() { });
        }

        [HttpGet]
        public async Task<IActionResult> GetTodasCategorias()
        {
            var result = await _repoCategoria.GetTodoAsync(PeliculasApiContext.RutaCategoriasApi);
            return Json(new { data = result  });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                //con token
                string token = HttpContext.Session.GetString("JWToken");
                //string token = null;
                await _repoCategoria.CrearAsync(PeliculasApiContext.RutaCategoriasApi, categoria, token);

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            Categoria itemCategoria = new Categoria();

            if (id == null)
            {
                return NotFound();
            }

            itemCategoria = await _repoCategoria.GetAsync(PeliculasApiContext.RutaCategoriasApi, id.GetValueOrDefault());

            if (itemCategoria == null)
            {
                return NotFound();
            }

            return View(itemCategoria);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                //con token
                //string token = HttpContext.Session.GetString("JWToken");
                string token = null;
                await _repoCategoria.ActualizarAsync(PeliculasApiContext.RutaCategoriasApi + categoria.Id, categoria, token);
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            //con token
            //string token = HttpContext.Session.GetString("JWToken");
            string token = null;
            var status = await _repoCategoria.BorrarAsync(PeliculasApiContext.RutaCategoriasApi, id, token);

            if (status)
            {
                return Json(new { success = true, message = "Borrado correctamente" });
            }

            return Json(new { success = false, message = "No se pudo borrar" });
        }
    }
}