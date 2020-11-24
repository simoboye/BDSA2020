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
            var actual = await repository.GetStudentAsync(1);

            var expected = new StudentDetailsDTO
            { 
                Id = 1, 
                Degree = Degree.Bachelor, 
                MinSalary = 100, 
                MinWorkingHours = 5, 
                MaxWorkingHours = 20, 
                Agreement = false, 
                Location = "Nowhere",
                PlacementDescriptions = new [] { new PlacementDescription { Id = 1 } }
            };

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Degree, actual.Degree);
            Assert.Equal(expected.PlacementDescriptions.First().Id, actual.PlacementDescriptions.First().Id);
        }

        [Fact]
        public async Task GetStudent_returns_ArgumentException_on_not_found_student() 
        {
            await Assert.ThrowsAsync<ArgumentException>(() => repository.GetStudentAsync(100));
        }

        [Fact]
        public async Task CreateStudent_returns_the_id_of_created_student()
        {
            var student = new CreateStudentDTO
            { 
                Degree = Degree.Bachelor, 
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

            Assert.Equal(lastId + 1, actual);
        }

        [Fact]
        public async Task DeleteStudent_returns_true() 
        {
            var actual = await repository.DeleteStudentAsync(1);

            Assert.True(actual);
        }

        [Fact]
        public async Task DeleteStudent_returns_ArgumentException_on_not_found()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => repository.DeleteStudentAsync(100));
        }

        [Fact]
        public async Task UpdateStudent_returns_true_on_updated()
        {
            var studentToUpdate = await Context.Students.FirstAsync();
            var dto = new UpdateStudentDTO
            {
                Id = studentToUpdate.Id,
                Keywords = new [] { new StudentKeywords { KeywordId = 7, StudentId = studentToUpdate.Id } },
                Degree = Degree.Other,
                MinSalary = 1,
                MinWorkingHours = 1,
                MaxWorkingHours = 2,
                Agreement = true,
                Location = "København"
            };
            var actual = await repository.UpdateStudentAsync(dto);

            Assert.True(actual);
        }

        [Fact]
        public async Task UpdateStudent_returns_ArgumentException_on_not_found()
        {
            var studentToUpdate = new UpdateStudentDTO { Id = 100 };
            await Assert.ThrowsAsync<ArgumentException>(() => repository.UpdateStudentAsync(studentToUpdate));
        }
    }
}
