using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BDSA2020.Api.Controllers;
using BDSA2020.Entities;
using BDSA2020.Models;
using BDSA2020.Shared;
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
                new Student { Id = 1 },
                new Student { Id = 2 }
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
        public async Task Get_returns_500_on_internal_error()
        {
            repository.Setup(r => r.GetStudentsAsync()).ThrowsAsync(new Exception());
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Get();

            var actionResult = Assert.IsType<ActionResult<IEnumerable<Student>>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(500, code.StatusCode);
        }

        [Fact]
        public async Task Get_given_id_returns_200_and_student()
        {
            var student = new Student { Id = 1, Degree = Degree.Bachelor, MinSalary = 100, MinWorkingHours = 5, MaxWorkingHours = 20, Agreement = false, Location = "Nowhere" };

            repository.Setup(r => r.GetStudentAsync(student.Id)).ReturnsAsync(student);
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Get(1);

            var actionResult = Assert.IsType<ActionResult<Student>>(actual);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var actualStudent = Assert.IsType<Student>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(student, actualStudent);
        }

        [Fact]
        public async Task Get_given_id_that_does_not_exist_returns_404()
        {
            repository.Setup(r => r.GetStudentAsync(100)).ThrowsAsync(new ArgumentException());
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Get(100);

            var actionResult = Assert.IsType<ActionResult<Student>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(404, code.StatusCode);
        }

        [Fact]
        public async Task Get_given_id_returns_500_on_internal_error()
        {
            repository.Setup(r => r.GetStudentAsync(1)).ThrowsAsync(new Exception());
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Get(1);

            var actionResult = Assert.IsType<ActionResult<Student>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(500, code.StatusCode);
        }

        [Fact]
        public async Task Create_returns_200_and_id_of_created_student()
        {
            var nextMockedId = 10;
            var student = new CreateStudentDTO();
            repository.Setup(r => r.CreateStudentAsync(student)).ReturnsAsync(nextMockedId);
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Create(student);

            var actionResult = Assert.IsType<ActionResult<int>>(actual);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var actualId = Assert.IsType<int>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(nextMockedId, actualId);
        }

        // [Fact]
        // public async Task Create_returns_409_on_conflict()
        // {
        //     var student = new CreateStudentDTO { Id = 1 };
        //     repository.Setup(r => r.CreateStudentAsync(student)).ThrowsAsync(new ArgumentException());
        //     var controller = new StudentRepositoryController(repository.Object);

        //     var actual = await controller.Create(student);

        //     var actionResult = Assert.IsType<ActionResult<int>>(actual);
        //     var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
        //     Assert.Equal(409, code.StatusCode);
        // }

        [Fact]
        public async Task Create_returns_500_on_internal_error()
        {
            var student = new CreateStudentDTO();
            repository.Setup(r => r.CreateStudentAsync(student)).ThrowsAsync(new Exception());
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Create(student);

            var actionResult = Assert.IsType<ActionResult<int>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(500, code.StatusCode);
        }

        [Fact]
        public async Task Delete_returns_200_and_true()
        {
            repository.Setup(r => r.DeleteStudentAsync(1)).ReturnsAsync(true);
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Delete(1);

            var actionResult = Assert.IsType<ActionResult<bool>>(actual);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var hasBeenDeleted = Assert.IsType<bool>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.True(hasBeenDeleted);
        }

        [Fact]
        public async Task Delete_returns_404_on_not_found()
        {
            repository.Setup(r => r.DeleteStudentAsync(10)).ThrowsAsync(new ArgumentException());
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Delete(10);

            var actionResult = Assert.IsType<ActionResult<bool>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(404, code.StatusCode);
        }

        [Fact]
        public async Task Delete_returns_500_on_internal_error()
        {
            repository.Setup(r => r.DeleteStudentAsync(1)).ThrowsAsync(new Exception());
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Delete(1);

            var actionResult = Assert.IsType<ActionResult<bool>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(500, code.StatusCode);
        }

        [Fact]
        public async Task Update_returns_200_and_true()
        {
            var student = new Student { Degree = Degree.Bachelor };
            repository.Setup(r => r.UpdateStudentAsync(student)).ReturnsAsync(true);
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Update(student);

            var actionResult = Assert.IsType<ActionResult<bool>>(actual);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var hasBeenUpdated = Assert.IsType<bool>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.True(hasBeenUpdated);
        }

        [Fact]
        public async Task Update_returns_404_on_not_found()
        {
            var student = new Student { Id = 1 };
            repository.Setup(r => r.UpdateStudentAsync(student)).ThrowsAsync(new ArgumentException());
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Update(student);

            var actionResult = Assert.IsType<ActionResult<bool>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(404, code.StatusCode);
        }

        [Fact]
        public async Task Update_returns_500_on_internal_error()
        {
            var student = new Student { Degree = Degree.Bachelor };
            repository.Setup(r => r.UpdateStudentAsync(student)).ThrowsAsync(new Exception());
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Update(student);

            var actionResult = Assert.IsType<ActionResult<bool>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(500, code.StatusCode);
        }
    }
}
