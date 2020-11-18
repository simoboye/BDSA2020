using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BDSA2020.Entities;
using BDSA2020.Shared;
using Microsoft.EntityFrameworkCore;

namespace BDSA2020.Models
{
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

        public async Task<Student> GetStudentAsync(int id)
        {
            var student = await context.Students.FindAsync(id);

            if (student == null)
            {
                throw new ArgumentException($"Could not find student with id {id}");
            }

            return student;

        }

        public async Task<int> CreateStudentAsync(CreateStudentDTO student)
        {
            var entity = new Student
            {
                Degree = student.Degree,
                Keywords = student.Keywords,
                MinSalary = student.MinSalary,
                MinWorkingHours = student.MinWorkingHours,
                MaxWorkingHours = student.MaxWorkingHours,
                Agreement = student.Agreement,
                Location = student.Location
            };

            await context.Students.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await GetStudentAsync(id);

            context.Students.Remove(student);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateStudentAsync(UpdateStudentDTO student)
        {
            var entity = await GetStudentAsync(student.Id);

            entity.Agreement = student.Agreement;
            entity.Degree = student.Degree;
            entity.Keywords = student.Keywords;
            entity.Location = student.Location;
            entity.MaxWorkingHours = student.MaxWorkingHours;
            entity.MinWorkingHours = student.MinWorkingHours;
            entity.MinSalary = student.MinSalary;

            await context.SaveChangesAsync();

            return true;
        }
    }
}
