using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BDSA2020.Entities;
using Microsoft.EntityFrameworkCore;

namespace BDSA2020.Models
{
    public interface IStudentRepository
    {
        Task<ICollection<Student>> GetStudentsAsync();
    }
    public class StudentRepository : IStudentRepository
    {
        private readonly IContext context;

        public StudentRepository(IContext context)
        {
            this.context = context;
        }

        public async Task<ICollection<Student>> GetStudentsAsync()
        {
            var students = context.Students;

            return await students.ToListAsync();

        }
    }
}
