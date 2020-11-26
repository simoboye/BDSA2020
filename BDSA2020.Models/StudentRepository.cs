using System;
using System.Linq;
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

        public async Task<ICollection<StudentDetailsDTO>> GetStudentsAsync()
        {
            var students = from s in context.Students
                           select new StudentDetailsDTO
                           {
                               Id = s.Id,
                               Degree = s.Degree,
                               KeywordNames = s.Keywords.Select(k => k.Keyword.Name).ToList(),
                               MinSalary = s.MinSalary,
                               MinWorkingHours = s.MinWorkingHours,
                               MaxWorkingHours = s.MaxWorkingHours,
                               Agreement = s.Agreement,
                               Location = s.Location,
                               PlacementDescriptionIds = s.PlacementDescriptions.Select(p => p.PlacementDescription.Id).ToList()
                           };

            return await students.ToListAsync();
        }

        public async Task<StudentDetailsDTO> GetStudentAsync(int id)
        {
            var studentQuery = from s in context.Students
                           where s.Id == id
                           select new StudentDetailsDTO
                           {
                               Id = s.Id,
                               Degree = s.Degree,
                               KeywordNames = s.Keywords.Select(k => k.Keyword.Name).ToList(),
                               MinSalary = s.MinSalary,
                               MinWorkingHours = s.MinWorkingHours,
                               MaxWorkingHours = s.MaxWorkingHours,
                               Agreement = s.Agreement,
                               Location = s.Location,
                               PlacementDescriptionIds = s.PlacementDescriptions.Select(p => p.PlacementDescription.Id).ToList()
                           };

            var student = await studentQuery.FirstOrDefaultAsync();

            if (student == null)
            {
                throw new ArgumentException($"Could not find student with id {id}.");
            }

            return student;
        }

        public async Task<int> CreateStudentAsync(CreateStudentDTO student)
        {
            var entity = new Student
            {
                Degree = student.Degree,
                Keywords = GetKeywords(student.KeywordNames).ToList(),
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
            var student = await context.Students.FindAsync(id);

            if (student == null)
            {
                throw new ArgumentException($"Could not remove student with id {id}, because it does not exist.");
            }

            context.Students.Remove(student);

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateStudentAsync(UpdateStudentDTO student)
        {
            var entity = await context.Students.FindAsync(student.Id);

            if (entity == null)
            {
                throw new ArgumentException($"Could not remove student with id {student.Id}, because it does not exist.");
            }

            // Remove the previous keywords, as they are a primary key
            var keywordsForStudent = from k in context.StudentKeywords
                                     where k.StudentId == entity.Id
                                     select k;

            context.StudentKeywords.RemoveRange(keywordsForStudent);

            entity.Agreement = student.Agreement;
            entity.Degree = student.Degree;
            entity.Keywords = GetKeywords(student.KeywordNames).ToList();
            entity.Location = student.Location;
            entity.MaxWorkingHours = student.MaxWorkingHours;
            entity.MinWorkingHours = student.MinWorkingHours;
            entity.MinSalary = student.MinSalary;

            await context.SaveChangesAsync();

            return true;
        }

        private IEnumerable<StudentKeyword> GetKeywords(IEnumerable<string> keywordNames) {
            var keywords = from k in context.Keywords
                           where keywordNames.Contains(k.Name)
                           select k;

            var keywordMap = keywords.ToDictionary(k => k.Name);

            foreach (var name in keywordNames)
            {
                if (keywordMap.TryGetValue(name, out Keyword keyword))
                {
                    yield return new StudentKeyword { Keyword = keyword };
                }
                else
                {
                    throw new ArgumentException($"Keyword with name {name} does not exit.");
                }
            }
        }
    }
}
