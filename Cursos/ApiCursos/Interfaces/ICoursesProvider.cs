using ApiCursos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCursos.Interfaces
{
    public interface ICoursesProvider
    {
        Task<ICollection<Course>> GetAllAsync();

        Task<ICollection<Course>> SearchAsync(string search);

        Task<Course> GetAsync(int id);

        Task<bool> UpdateAsync(int id, Course course);

        Task<(bool IsSuccess, int? Id)> AddAsync(Course course);
    }
}
