using System;
using System.Linq;
using System.Threading.Tasks;
using BDSA2020.Entities;
using BDSA2020.Shared;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BDSA2020.Models.Tests
{
    public class StudentRepositoryTests : SqlLiteContext
    {
        private readonly IStudentRepository repository;

        public StudentRepositoryTests() : base()
        {
            repository = new StudentRepository(Context);
        }

        [Fact]
        public async Task GetStudentsAsync_returns_list_of_students()
        {
            // Act
            var actual = await repository.GetStudentsAsync();

            // Assert
            Assert.Equal(4, actual.Count);
        }

        [Fact]
        public async Task GetStudent_returns_the_requested_student() 
        {
            var entity = await Context.Students.FirstOrDefaultAsync();
            var actual = await repository.GetStudentAsync(entity.Id);

            var expected = new StudentDetailsDTO
            { 
                Id = entity.Id,
                Degree = Degree.Bachelor, 
                KeywordNames = new [] { "Testing", "C#" },
                MinSalary = 100, 
                MinWorkingHours = 5, 
                MaxWorkingHours = 20, 
                Agreement = false, 
                Location = "Nowhere",
                PlacementDescriptionIds = new [] { 1 }
            };

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Degree, actual.Degree);
            Assert.Equal(expected.PlacementDescriptionIds.First(), actual.PlacementDescriptionIds.First());
            Assert.Equal(expected.KeywordNames, actual.KeywordNames);
        }

        [Fact]
        public async Task GetStudent_returns_ArgumentException_on_not_found_student() 
        {
            await Assert.ThrowsAsync<ArgumentException>(() => repository.GetStudentAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task CreateStudent_returns_the_id_of_created_student()
        {
            var id = Guid.NewGuid();
            var student = new CreateStudentDTO
            { 
                Id = id,
                Degree = Degree.Bachelor,
                KeywordNames = new [] { "Testing" },
                MinSalary = 100, 
                MinWorkingHours = 5, 
                MaxWorkingHours = 20, 
                Agreement = false, 
                Location = "Nowhere" 
            };

            var studentsList = await Context.Students.ToListAsync();
            var lastId = studentsList.OrderByDescending(s => s.Id)
                                        .FirstOrDefault()
                                        .Id;

            var actual = await repository.CreateStudentAsync(student);
        
        
            Assert.Equal(student.Id, actual);
        }

        [Fact]
        public async Task DeleteStudent_returns_true() 
        {
            var entity = await Context.Students.FirstOrDefaultAsync();
            var actual = await repository.DeleteStudentAsync(entity.Id);

            Assert.True(actual);
        }

        [Fact]
        public async Task DeleteStudent_returns_ArgumentException_on_not_found()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => repository.DeleteStudentAsync(Guid.NewGuid()));
        }

        [Fact]
        public async Task UpdateStudent_returns_true_on_updated()
        {
            var studentToUpdate = await Context.Students.FirstAsync();
            var dto = new UpdateStudentDTO
            {
                Id = studentToUpdate.Id,
                KeywordNames = new [] { "Testing", "C#" },
                Degree = Degree.Other,
                MinSalary = 1,
                MinWorkingHours = 1,
                MaxWorkingHours = 2,
                Agreement = true,
                Location = "København"
            };
            var actual = await repository.UpdateStudentAsync(dto);

            Assert.True(actual);

            var updatedStudent = await Context.Students.FirstAsync();
            Assert.Equal(dto.Id, updatedStudent.Id);
            Assert.Equal(new [] { "Testing", "C#" }, updatedStudent.Keywords.Select(k => k.Keyword.Name).ToList());
        }

        [Fact]
        public async Task UpdateStudent_returns_ArgumentException_on_not_found()
        {
            var studentToUpdate = new UpdateStudentDTO { Id = Guid.NewGuid() };
            await Assert.ThrowsAsync<ArgumentException>(() => repository.UpdateStudentAsync(studentToUpdate));
        }
    }
}
