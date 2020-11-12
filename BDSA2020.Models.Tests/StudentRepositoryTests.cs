using System;
using System.Linq;
using System.Threading.Tasks;
using BDSA2020.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BDSA2020.Models.Tests
{
    public class StudentRepositoryTests
    {
        private readonly SqliteConnection connection;
        private readonly Context context;
        private readonly StudentRepository repository;

        public StudentRepositoryTests()
        {
            // Arrange
            connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<Context>().UseSqlite(connection);
            context = new Context(builder.Options);
            context.Database.EnsureCreated();
            // context.GenerateTestData();

            repository = new StudentRepository(context);
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

            var expected = new Student 
            { 
                Id = 1, 
                Degree = Degree.Bachelor, 
                MinSalary = 100, 
                MinWorkingHours = 5, 
                MaxWorkingHours = 20, 
                Agreement = false, 
                Location = "Nowhere" 
            };

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Degree, actual.Degree);
        }

        [Fact]
        public async Task GetStudent_returns_ArgumentException_on_not_found_student() 
        {
            await Assert.ThrowsAsync<ArgumentException>(() => repository.GetStudentAsync(100));
        }

        [Fact]
        public async Task CreateStudent_returns_the_id_of_created_student()
        {
            var student = new Student 
            { 
                Degree = Degree.Bachelor, 
                MinSalary = 100, 
                MinWorkingHours = 5, 
                MaxWorkingHours = 20, 
                Agreement = false, 
                Location = "Nowhere" 
            };

            var studentsList = await context.Students.ToListAsync();
            var expected = studentsList.OrderByDescending(s => s.Id)
                                        .FirstOrDefault();

            var actual = await repository.CreateStudentAsync(student);

            Assert.Equal(expected.Id + 1, actual);
        }

        [Fact]
        public async Task CreateStudent_returns_ArguementException_on_conflict()
        {
            var student = new Student 
            { 
                Id = 1,
                Degree = Degree.Bachelor, 
                MinSalary = 100, 
                MinWorkingHours = 5, 
                MaxWorkingHours = 20, 
                Agreement = false, 
                Location = "Nowhere" 
            };
            await Assert.ThrowsAsync<ArgumentException>(() => repository.CreateStudentAsync(student));
        }

        [Fact]
        public async Task DeleteStudent_returns_true() 
        {
            var actual = await repository.DeleteStudentAsync(1);

            Assert.True(actual);
        }
    }
}
