using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BDSA2020.Entities;
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
                throw new ArgumentException("The student doesn't exist");
            }

            return student;

        }

        public async Task<int> CreateStudentAsync(Student student)
        {
            var checkIfConflict = await context.Students.FindAsync(student.Id);

            if (checkIfConflict != null)
            {
                throw new ArgumentException("The student already exists");
            }

            await context.Students.AddAsync(student);
            await context.SaveChangesAsync();

            return student.Id;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await context.Students.FindAsync(id);

            if (student == null) 
            {
                throw new ArgumentException($"Could not find student with id {id}");
            }

            context.Students.Remove(student);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateStudentAsync(Student student)
        {
            var entity = await context.Students.FindAsync(student.Id);

            if (entity == null)
            {
                throw new ArgumentException("Student does not exist");
            }

            entity.Agreement = student.Agreement;
            entity.Degree = student.Degree;
            entity.Keywords = student.Keywords;
            entity.Location = student.Location;
            entity.MaxWorkingHours = student.MaxWorkingHours;
            entity.MinWorkingHours = student.MinWorkingHours;
            entity.MinSalary = student.MinSalary;
            entity.PlacementDescriptions = student.PlacementDescriptions;

            await context.SaveChangesAsync();

            return true;
        }
    }
}
