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
                new StudentDetailsDTO { Id = Guid.NewGuid(), KeywordNames = new [] { "Testing" } },
                new StudentDetailsDTO { Id = Guid.NewGuid(), KeywordNames = new [] { "C#" } }
            };

            repository.Setup(r => r.GetStudentsAsync()).ReturnsAsync(students);
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Get(true);

            var actionResult = Assert.IsType<ActionResult<IEnumerable<StudentDetailsDTO>>>(actual);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var actualStudents = Assert.IsType<List<StudentDetailsDTO>>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(students.Length, actualStudents.Count());
        }

        [Fact]
        public async Task Get_returns_500_on_internal_error()
        {
            repository.Setup(r => r.GetStudentsAsync()).ThrowsAsync(new Exception());
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Get(true);

            var actionResult = Assert.IsType<ActionResult<IEnumerable<StudentDetailsDTO>>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(500, code.StatusCode);
        }

        [Fact]
        public async Task Get_given_id_returns_200_and_student()
        {
            var student = new StudentDetailsDTO { Id = Guid.NewGuid(), Degree = Degree.Bachelor, MinSalary = 100, MinWorkingHours = 5, MaxWorkingHours = 20, Agreement = false, Location = "Nowhere" };

            repository.Setup(r => r.GetStudentAsync(student.Id)).ReturnsAsync(student);
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Get(student.Id, true);

            var actionResult = Assert.IsType<ActionResult<StudentDetailsDTO>>(actual);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var actualStudent = Assert.IsType<StudentDetailsDTO>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(student, actualStudent);
        }

        [Fact]
        public async Task Get_given_id_that_does_not_exist_returns_404()
        {
            var id = Guid.NewGuid();
            repository.Setup(r => r.GetStudentAsync(id)).ThrowsAsync(new ArgumentException());
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Get(id, true);

            var actionResult = Assert.IsType<ActionResult<StudentDetailsDTO>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(404, code.StatusCode);
        }

        [Fact]
        public async Task Get_given_id_returns_500_on_internal_error()
        {
            var id = Guid.NewGuid();
            repository.Setup(r => r.GetStudentAsync(id)).ThrowsAsync(new Exception());
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Get(id, true);

            var actionResult = Assert.IsType<ActionResult<StudentDetailsDTO>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(500, code.StatusCode);
        }

        [Fact]
        public async Task Create_returns_200_and_id_of_created_student()
        {
            var nextMockedId = Guid.NewGuid();
            var student = new CreateStudentDTO();
            repository.Setup(r => r.CreateStudentAsync(student)).ReturnsAsync(nextMockedId);
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Create(student, true);

            var actionResult = Assert.IsType<ActionResult<Guid>>(actual);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var actualId = Assert.IsType<Guid>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(nextMockedId, actualId);
        }

        [Fact]
        public async Task Create_returns_500_on_internal_error()
        {
            var student = new CreateStudentDTO();
            repository.Setup(r => r.CreateStudentAsync(student)).ThrowsAsync(new Exception());
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Create(student, true);

            var actionResult = Assert.IsType<ActionResult<Guid>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(500, code.StatusCode);
        }

        [Fact]
        public async Task Delete_returns_200_and_true()
        {
            var id = Guid.NewGuid();
            repository.Setup(r => r.DeleteStudentAsync(id)).ReturnsAsync(true);
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Delete(id, true);

            var actionResult = Assert.IsType<ActionResult<bool>>(actual);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var hasBeenDeleted = Assert.IsType<bool>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.True(hasBeenDeleted);
        }

        [Fact]
        public async Task Delete_returns_404_on_not_found()
        {
            var id = Guid.NewGuid();
            repository.Setup(r => r.DeleteStudentAsync(id)).ThrowsAsync(new ArgumentException());
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Delete(id, true);

            var actionResult = Assert.IsType<ActionResult<bool>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(404, code.StatusCode);
        }

        [Fact]
        public async Task Delete_returns_500_on_internal_error()
        {
            var id = Guid.NewGuid();
            repository.Setup(r => r.DeleteStudentAsync(id)).ThrowsAsync(new Exception());
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Delete(id, true);

            var actionResult = Assert.IsType<ActionResult<bool>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(500, code.StatusCode);
        }

        [Fact]
        public async Task Update_returns_200_and_true()
        {
            var student = new UpdateStudentDTO { Degree = Degree.Bachelor };
            repository.Setup(r => r.UpdateStudentAsync(student)).ReturnsAsync(true);
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Update(student, true);

            var actionResult = Assert.IsType<ActionResult<bool>>(actual);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var hasBeenUpdated = Assert.IsType<bool>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.True(hasBeenUpdated);
        }

        [Fact]
        public async Task Update_returns_404_on_not_found()
        {
            var student = new UpdateStudentDTO { Id = Guid.NewGuid() };
            repository.Setup(r => r.UpdateStudentAsync(student)).ThrowsAsync(new ArgumentException());
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Update(student, true);

            var actionResult = Assert.IsType<ActionResult<bool>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(404, code.StatusCode);
        }

        [Fact]
        public async Task Update_returns_500_on_internal_error()
        {
            var student = new UpdateStudentDTO { Degree = Degree.Bachelor };
            repository.Setup(r => r.UpdateStudentAsync(student)).ThrowsAsync(new Exception());
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Update(student, true);

            var actionResult = Assert.IsType<ActionResult<bool>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(500, code.StatusCode);
        }

        [Fact]
        public async Task Save_returns_true_on_200()
        {
            var studentId = new Guid("5a87427d-f0af-421d-a340-7d9dd8f9f76e");
            var descriptionId = 1;
            repository.Setup(r => r.SavePlacementDescription(studentId, descriptionId)).ReturnsAsync(true);
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Save(studentId, descriptionId, true);

            var actionCodeResult = Assert.IsType<ActionResult<bool>>(actual);
            var code = Assert.IsType<OkObjectResult>(actionCodeResult.Result);
            var hasBeenSaved = Assert.IsType<bool>(code.Value);

            Assert.Equal(200, code.StatusCode);
            Assert.True(hasBeenSaved);
        }

        [Fact]
        public async Task Save_returns_404_on_not_found()
        {
            var studentId = new Guid("5a87427d-f0af-421d-a340-7d9dd8f9f76e");
            var descriptionId = 100;
            repository.Setup(r => r.SavePlacementDescription(studentId, descriptionId)).ThrowsAsync(new ArgumentException());
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Save(studentId, descriptionId, true);

            var actionCodeResult = Assert.IsType<ActionResult<bool>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionCodeResult.Result);

            Assert.Equal(404, code.StatusCode);
        }

        [Fact]
        public async Task Save_returns_500_on_internal_server_error()
        {
            var studentId = new Guid("5a87427d-f0af-421d-a340-7d9dd8f9f76e");
            var descriptionId = 1;
            repository.Setup(r => r.SavePlacementDescription(studentId, descriptionId)).ThrowsAsync(new Exception());
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.Save(studentId, descriptionId, true);

            var actionCodeResult = Assert.IsType<ActionResult<bool>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionCodeResult.Result);

            Assert.Equal(500, code.StatusCode);
        }

        [Fact]
        public async Task UnSave_returns_true_on_200()
        {
            var studentId = new Guid("a3aaf097-a515-4aac-9a90-c26ee1e40488");
            var descriptionId = 1;
            repository.Setup(r => r.UnSavePlacementDescription(studentId, descriptionId)).ReturnsAsync(true);
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.UnSave(studentId, descriptionId, true);

            var actionCodeResult = Assert.IsType<ActionResult<bool>>(actual);
            var code = Assert.IsType<OkObjectResult>(actionCodeResult.Result);
            var hasBeenUnSaved = Assert.IsType<bool>(code.Value);

            Assert.Equal(200, code.StatusCode);
            Assert.True(hasBeenUnSaved);
        }

        [Fact]
        public async Task UnSave_returns_404_on_not_found()
        {
            var studentId = new Guid("5a87427d-f0af-421d-a340-7d9dd8f9f76e");
            var descriptionId = 100;
            repository.Setup(r => r.UnSavePlacementDescription(studentId, descriptionId)).ThrowsAsync(new ArgumentException());
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.UnSave(studentId, descriptionId, true);

            var actionCodeResult = Assert.IsType<ActionResult<bool>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionCodeResult.Result);

            Assert.Equal(404, code.StatusCode);
        }

        [Fact]
        public async Task UnSave_returns_500_on_internal_server_error()
        {
            var studentId = new Guid("5a87427d-f0af-421d-a340-7d9dd8f9f76e");
            var descriptionId = 1;
            repository.Setup(r => r.UnSavePlacementDescription(studentId, descriptionId)).ThrowsAsync(new Exception());
            var controller = new StudentRepositoryController(repository.Object);

            var actual = await controller.UnSave(studentId, descriptionId, true);

            var actionCodeResult = Assert.IsType<ActionResult<bool>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionCodeResult.Result);

            Assert.Equal(500, code.StatusCode);
        }
    }
}
