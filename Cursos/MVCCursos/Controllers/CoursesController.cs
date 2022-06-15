using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCCursos.Interfaces;
using MVCCursos.Models;

namespace MVCCursos.Controllers
{
    public class CoursesController : Controller
    {             
        private readonly ICoursesProvider coursesProvider;
        public List<Course> Courses { get; set; }

        public CoursesController(ICoursesProvider coursesProvider)
        {
            this.coursesProvider = coursesProvider;
        }                
        public async Task<IActionResult> Index()
        {
            var results = await coursesProvider.GetAllAsync();
            return View(results);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await coursesProvider.GetAsync(id.Value);            
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await coursesProvider.AddAsync(course);
                    if (result.IsSuccess)
                    {                        
                        return RedirectToAction(nameof(Index));
                    }                    
                }
                /*catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe esta marca.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }*/
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(course);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await coursesProvider.GetAsync(id.Value);                        
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await coursesProvider.UpdateAsync(course.Id, course);
                    if (result)
                    {
                        return RedirectToAction(nameof(Index));                        
                    }                                        
                }
                /*catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe esta marca.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }*/
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(course);
        }
                
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await coursesProvider.GetAsync(id.Value);
            if (course == null)
            {
                return NotFound();
            }

            var result = await coursesProvider.DeleteAsync(course.Id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }

            //_context.Brands.Remove(brand);
            return RedirectToAction(nameof(Index));
        }
    }
}