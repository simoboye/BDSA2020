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
    public class PlacementDescriptionRepositoryControllerTests
    {
        private readonly Mock<IPlacementDescriptionRepository> repository;

        public PlacementDescriptionRepositoryControllerTests()
        {
            var repository = new Mock<IPlacementDescriptionRepository>();
            this.repository = repository;
        }

        [Fact]
        public async Task Get_returns_200_and_students()
        {
            var descriptions = new []
            {
                new PlacementDescription { Id = 1 },
                new PlacementDescription { Id = 2 }
            };

            repository.Setup(r => r.GetPlacementDescriptionsAsync()).ReturnsAsync(descriptions);
            var controller = new PlacementDescriptionRepositoryController(repository.Object);

            var actual = await controller.Get();

            var actionResult = Assert.IsType<ActionResult<IEnumerable<PlacementDescription>>>(actual);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var actualDescriptions = Assert.IsType<List<PlacementDescription>>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(descriptions.Length, actualDescriptions.Count());
        }

        [Fact]
        public async Task Get_returns_500_on_internal_error()
        {
            repository.Setup(r => r.GetPlacementDescriptionsAsync()).ThrowsAsync(new Exception());
            var controller = new PlacementDescriptionRepositoryController(repository.Object);

            var actual = await controller.Get();

            var actionResult = Assert.IsType<ActionResult<IEnumerable<PlacementDescription>>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(500, code.StatusCode);
        }

        [Fact]
        public async Task Get_given_id_returns_200_and_student()
        {
            var description = new PlacementDescription { Id = 1, Degree = Degree.Bachelor, MinSalary = 100, MinWorkingHours = 5, MaxWorkingHours = 20, Agreement = false, Location = "Nowhere" };

            repository.Setup(r => r.GetPlacementDescriptionAsync(description.Id)).ReturnsAsync(description);
            var controller = new PlacementDescriptionRepositoryController(repository.Object);

            var actual = await controller.Get(1);

            var actionResult = Assert.IsType<ActionResult<PlacementDescription>>(actual);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var actualDescription = Assert.IsType<PlacementDescription>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(description, actualDescription);
        }

        [Fact]
        public async Task Get_given_id_that_does_not_exist_returns_404()
        {
            repository.Setup(r => r.GetPlacementDescriptionAsync(100)).ThrowsAsync(new ArgumentException());
            var controller = new PlacementDescriptionRepositoryController(repository.Object);

            var actual = await controller.Get(100);

            var actionResult = Assert.IsType<ActionResult<PlacementDescription>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(404, code.StatusCode);
        }

        [Fact]
        public async Task Get_given_id_returns_500_on_internal_error()
        {
            repository.Setup(r => r.GetPlacementDescriptionAsync(1)).ThrowsAsync(new Exception());
            var controller = new PlacementDescriptionRepositoryController(repository.Object);

            var actual = await controller.Get(1);

            var actionResult = Assert.IsType<ActionResult<PlacementDescription>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(500, code.StatusCode);
        }

        [Fact]
        public async Task Create_returns_200_and_id_of_created_student()
        {
            var nextMockedId = 10;
            var description = new PlacementDescription();
            repository.Setup(r => r.CreatePlacementDescriptionAsync(description)).ReturnsAsync(nextMockedId);
            var controller = new PlacementDescriptionRepositoryController(repository.Object);

            var actual = await controller.Create(description);

            var actionResult = Assert.IsType<ActionResult<int>>(actual);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var actualId = Assert.IsType<int>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(nextMockedId, actualId);
        }

        [Fact]
        public async Task Create_returns_500_on_internal_error()
        {
            var description = new PlacementDescription();
            repository.Setup(r => r.CreatePlacementDescriptionAsync(description)).ThrowsAsync(new Exception());
            var controller = new PlacementDescriptionRepositoryController(repository.Object);

            var actual = await controller.Create(description);

            var actionResult = Assert.IsType<ActionResult<int>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(500, code.StatusCode);
        }

        [Fact]
        public async Task Delete_returns_200_and_true()
        {
            repository.Setup(r => r.DeletePlacementDescriptionAsync(1)).ReturnsAsync(true);
            var controller = new PlacementDescriptionRepositoryController(repository.Object);

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
            repository.Setup(r => r.DeletePlacementDescriptionAsync(10)).ThrowsAsync(new ArgumentException());
            var controller = new PlacementDescriptionRepositoryController(repository.Object);

            var actual = await controller.Delete(10);

            var actionResult = Assert.IsType<ActionResult<bool>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(404, code.StatusCode);
        }

        [Fact]
        public async Task Delete_returns_500_on_internal_error()
        {
            repository.Setup(r => r.DeletePlacementDescriptionAsync(1)).ThrowsAsync(new Exception());
            var controller = new PlacementDescriptionRepositoryController(repository.Object);

            var actual = await controller.Delete(1);

            var actionResult = Assert.IsType<ActionResult<bool>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(500, code.StatusCode);
        }

        [Fact]
        public async Task Update_returns_200_and_true()
        {
            var description = new PlacementDescription { Degree = Degree.Bachelor };
            repository.Setup(r => r.UpdatePlacementDescriptionAsync(description)).ReturnsAsync(true);
            var controller = new PlacementDescriptionRepositoryController(repository.Object);

            var actual = await controller.Update(description);

            var actionResult = Assert.IsType<ActionResult<bool>>(actual);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var hasBeenUpdated = Assert.IsType<bool>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.True(hasBeenUpdated);
        }

        [Fact]
        public async Task Update_returns_404_on_not_found()
        {
            var description = new PlacementDescription { Degree = Degree.Bachelor };
            repository.Setup(r => r.UpdatePlacementDescriptionAsync(description)).ThrowsAsync(new ArgumentException());
            var controller = new PlacementDescriptionRepositoryController(repository.Object);

            var actual = await controller.Update(description);

            var actionResult = Assert.IsType<ActionResult<bool>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(404, code.StatusCode);
        }

        [Fact]
        public async Task Update_returns_500_on_internal_error()
        {
            var description = new PlacementDescription { Degree = Degree.Bachelor };
            repository.Setup(r => r.UpdatePlacementDescriptionAsync(description)).ThrowsAsync(new Exception());
            var controller = new PlacementDescriptionRepositoryController(repository.Object);

            var actual = await controller.Update(description);

            var actionResult = Assert.IsType<ActionResult<bool>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(500, code.StatusCode);
        }
    }
}
