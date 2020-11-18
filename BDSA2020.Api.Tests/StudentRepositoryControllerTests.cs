using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BDSA2020.Api.Controllers;
using BDSA2020.Entities;
using BDSA2020.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BDSA2020.Api.Tests
{
    ///<summary>
    /// How to test ActionResults.
    /// See <see href="https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/testing?view=aspnetcore-5.0">here for more info</see>.
    ///</summary>
    public class StudentRepositoryControllerTests
    {
        private readonly Mock<IStudentRepository> repository;

        public StudentRepositoryControllerTests()
        {
            var repository = new Mock<IStudentRepository>();
            this.repository = repository;
        }

        [Fact]
        public async Task Get_returns_200_and_students()
        {
            var students = new []
            {
                new Student { Id = 1, Degree = Degree.Bachelor },
                new Student { Id = 2, Degree = Degree.Master }
            };

            repository.Setup(r => r.GetStudentsAsync()).ReturnsAsync(students);
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Get();

            var actionResult = Assert.IsType<ActionResult<IEnumerable<Student>>>(actual);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var actualStudents = Assert.IsType<List<Student>>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(students.Length, actualStudents.Count());
        }

        [Fact]
        public async Task Get_returns_404_on_internal_error()
        {
            repository.Setup(r => r.GetStudentsAsync()).ThrowsAsync(new Exception());
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Get();

            var actionResult = Assert.IsType<ActionResult<IEnumerable<Student>>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(500, code.StatusCode);
        }
    }
}
