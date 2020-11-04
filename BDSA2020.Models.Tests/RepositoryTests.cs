using System;
using System.Threading.Tasks;
using BDSA2020.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BDSA2020.Models.Tests
{
    public class RepositoryTests
    {
        private readonly SqliteConnection connection;
        private readonly Context context;
        private readonly Repository repository;

        public RepositoryTests()
        {
            // Arrange
            connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            var builder = new DbContextOptionsBuilder<Context>().UseSqlite(connection);
            context = new Context(builder.Options);
            context.Database.EnsureCreated();
            // context.GenerateTestData();

            repository = new Repository(context);
        }

        [Fact]
        public async Task GetStudentsAsync_returns_list_of_students()
        {
            // Act
            var actual = await repository.GetStudentsAsync();

            // Assert
            Assert.Equal(4, actual.Count);
        }
    }
}
