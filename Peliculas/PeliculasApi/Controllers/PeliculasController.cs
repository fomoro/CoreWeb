using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeliculasApi.Models;
using PeliculasApi.Models.Dtos;
using PeliculasApi.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PeliculasApi.Controllers
{
    [Authorize]
    [Route("api/Peliculas")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiPeliculas")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class PeliculasController : Controller
    {
        private readonly IPeliculaRepository _pelRepo;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;

        public PeliculasController(IPeliculaRepository pelRepo, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _pelRepo = pelRepo;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Obtener todas las peliculas
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetPeliculas()
        {
            var listaPeliculas = _pelRepo.GetPeliculas();

            var listaPeliculasDto = new List<PeliculaDto>();

            foreach (var lista in listaPeliculas)
            {
                listaPeliculasDto.Add(_mapper.Map<PeliculaDto>(lista));
            }
            return Ok(listaPeliculasDto);
        }

        /// <summary>
        /// Obtener una pelicula individual
        /// </summary>
        /// <param name="peliculaId"> Este es el id de la pelicula</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{peliculaId:int}", Name = "GetPelicula")]
        public IActionResult GetPelicula(int peliculaId)
        {
            var itemPelicula = _pelRepo.GetPelicula(peliculaId);

            if (itemPelicula == null)
            {
                return NotFound();
            }

            var itemPeliculaDto = _mapper.Map<PeliculaDto>(itemPelicula);
            return Ok(itemPeliculaDto);
        }

        /// <summary>
        /// Obtener peliculas por categoria
        /// </summary>
        /// <param name="categoriaId"> Este es el id de la categoría</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetPeliculasEnCategoria/{categoriaId:int}")]
        public IActionResult GetPeliculasEnCategoria(int categoriaId)
        {
            var listaPelicula = _pelRepo.GetPeliculasEnCategoria(categoriaId);

            if (listaPelicula == null)
            {
                return NotFound();
            }

            var itemPelicula = new List<PeliculaDto>();
            foreach (var item in listaPelicula)
            {
                itemPelicula.Add(_mapper.Map<PeliculaDto>(item));
            }

            return Ok(itemPelicula);
        }

        /// <summary>
        /// Buscar peliculas por nombre o descripción
        /// </summary>
        /// <param name="nombre"> Este es el nombre a buscar</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Buscar")]
        public IActionResult Buscar(string nombre)
        {
            try
            {
                var resultado = _pelRepo.BuscarPelicula(nombre);
                if (resultado.Any())
                {
                    return Ok(resultado);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos de la aplicación");
            }
        }

        /// <summary>
        /// Crear una nueva pelicula
        /// </summary>
        /// <param name="peliculaDto"></param>
        /// <returns></returns>       
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PeliculaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public IActionResult CrearPelicula([FromForm] PeliculaCreateDto peliculaDto)
        public IActionResult CrearPelicula([FromBody] PeliculaDto peliculaDto)
        {
            if (peliculaDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_pelRepo.ExistePelicula(peliculaDto.Nombre))
            {
                ModelState.AddModelError("", "La película ya existe");
                return StatusCode(404, ModelState);
            }

            /////*subida de archivos*/
            //var archivo = peliculaDto.Foto;
            //string rutaPrincipal = _hostingEnvironment.WebRootPath;
            //var archivos = HttpContext.Request.Form.Files;

            //if (archivos != null)
            //{
            //    //Nueva imagen
            //    var nombreFoto = Guid.NewGuid().ToString();
            //    var subidas = Path.Combine(rutaPrincipal, @"fotos");
            //    var extension = Path.GetExtension(archivos[0].FileName);

            //    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreFoto + extension), FileMode.Create))
            //    {
            //        archivos[0].CopyTo(fileStreams);
            //    }
            //    peliculaDto.RutaImagen = @"\fotos\" + nombreFoto + extension;
            //}

            var pelicula = _mapper.Map<Pelicula>(peliculaDto);

            if (!_pelRepo.CrearPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro{pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetPelicula", new { peliculaId = pelicula.Id }, pelicula);
        }

        /// <summary>
        /// Actualizar una película existente
        /// </summary>        
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPatch("{peliculaId:int}", Name = "ActualizarPelicula")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarPelicula(int peliculaId, [FromBody] PeliculaDto peliculaDto)
        {
            if (peliculaDto == null || peliculaId != peliculaDto.Id)
            {
                return BadRequest(ModelState);
            }

            var pelicula = _mapper.Map<Pelicula>(peliculaDto);

            if (!_pelRepo.ActualizarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro{pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Borrar una pelicula existente
        /// </summary>
        /// <param name="peliculaId"> Este es el id de la película</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpDelete("{peliculaId:int}", Name = "BorrarPelicula")]
        public IActionResult BorrarPelicula(int peliculaId)
        {
            if (!_pelRepo.ExistePelicula(peliculaId))
            {
                return NotFound();
            }

            var pelicula = _pelRepo.GetPelicula(peliculaId);

            if (!_pelRepo.BorrarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal borrando el registro{pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}