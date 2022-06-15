using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PeliculasWeb.Contexts;
using PeliculasWeb.Models;
using PeliculasWeb.Models.ViewModels;
using PeliculasWeb.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PeliculasWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountRepositorio _accRepo;
        private readonly ICategoriaRepositorio _repoCategoria;
        private readonly IPeliculaRepositorio _repoPelicula;

        public HomeController(ILogger<HomeController> logger, IAccountRepositorio accRepo, ICategoriaRepositorio repoCategoria, IPeliculaRepositorio repoPelicula)
        {
            _logger = logger;
            _accRepo = accRepo;
            _repoCategoria = repoCategoria;
            _repoPelicula = repoPelicula;
        }     

        
        public async Task<IActionResult> Index()
        //public IActionResult Index()
        {
            IndexVM listapeliculasCategorias = new IndexVM()
            {
                ListaCategorias = (IEnumerable<Categoria>)await _repoCategoria.GetTodoAsync(PeliculasApiContext.RutaCategoriasApi),
                ListaPeliculas = (IEnumerable<Pelicula>)await _repoPelicula.GetTodoAsync(PeliculasApiContext.RutaPeliculasApi),
            };
            return View(listapeliculasCategorias);

            //return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> IndexCategoria(int id)
        {
            var pelisEnCategoria = await _repoPelicula.GetPeliculasEnCategoriaAsync(PeliculasApiContext.RutaPeliculasEnCategoriaApi, id);
            return View(pelisEnCategoria);
        }

        public async Task<IActionResult> IndexBusqueda(string nombre)
        {
            var pelisEncontradas = await _repoPelicula.Buscar(PeliculasApiContext.RutaPeliculasApiBusqueda, nombre);
            return View(pelisEncontradas);
        }

        [HttpGet]
        public IActionResult Login()
        {
            UsuarioM usuario = new UsuarioM();
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UsuarioM obj)
        {
            if (ModelState.IsValid)
            {
                UsuarioM objUser = await _accRepo.LoginAsync(PeliculasApiContext.RutaUsuariosApi + "Login", obj);
                if (objUser.Token == null)
                {
                    TempData["alert"] = "Los datos son incorrectos";
                    return View();
                }

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                var name = ClaimTypes.Name;
                var user = objUser;

                identity.AddClaim(new Claim(ClaimTypes.Name, objUser.Token));

                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                HttpContext.Session.SetString("JWToken", objUser.Token);
                TempData["alert"] = "Bienvenido/a " + objUser.Usuario;
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }


        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(UsuarioM obj)
        {
            bool result = await _accRepo.RegisterAsync(PeliculasApiContext.RutaUsuariosApi + "Registro", obj);
            if (result == false)
            {
                return View();
            }

            TempData["alert"] = "Registro correcto";
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("JWToken", "");
            return RedirectToAction("Index");
        }        
    }
}
