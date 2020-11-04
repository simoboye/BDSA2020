using System;
using System.Threading.Tasks;
using BDSA2020.Api.Controllers;
using BDSA2020.Entities;
using BDSA2020.Models;
using Moq;
using Xunit;

namespace BDSA2020.Api.Tests
{
    public class RepositoryControllerTests
    {
        [Fact]
        public async Task Get_student_returns_students()
        {
            var students = new []
            {
                new Student { Id = 1, Degree = Degree.Bachelor },
                new Student { Id = 2, Degree = Degree.Master }
            };

            var repository = new Mock<IRepository>();
            repository.Setup(r => r.GetStudentsAsync()).ReturnsAsync(students);

            var controller = new RepositoryController(repository.Object);

            var actual = await controller.Get();

            Assert.Equal(students, actual);
        }
    }
}
