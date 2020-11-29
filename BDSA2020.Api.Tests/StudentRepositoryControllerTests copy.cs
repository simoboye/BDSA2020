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
    public class CompanyRepositoryControllerTests
    {
        private readonly Mock<ICompanyRepository> repository;

        public CompanyRepositoryControllerTests()
        {
            var repository = new Mock<ICompanyRepository>();
            this.repository = repository;
        }

        [Fact]
        public async Task Get_returns_200_and_company()
        {
            var companies = new []
            {
                new CompanyDetailsDTO { Id = Guid.NewGuid()},
                new CompanyDetailsDTO { Id = Guid.NewGuid()}
            };

            repository.Setup(r => r.GetCompaniesAsync()).ReturnsAsync(companies);
            var controller = new CompanyRepositoryController(repository.Object);

            var actual = await controller.Get(true);

            var actionResult = Assert.IsType<ActionResult<IEnumerable<CompanyDetailsDTO>>>(actual);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var actualCompanies = Assert.IsType<List<CompanyDetailsDTO>>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(companies.Length, actualCompanies.Count());
        }

        [Fact]
        public async Task Get_returns_500_on_internal_error()
        {
            repository.Setup(r => r.GetCompaniesAsync()).ThrowsAsync(new Exception());
            var controller = new CompanyRepositoryController(repository.Object);

            var actual = await controller.Get(true);

            var actionResult = Assert.IsType<ActionResult<IEnumerable<CompanyDetailsDTO>>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(500, code.StatusCode);
        }

        [Fact]
        public async Task Get_given_id_returns_200_and_company()
        {
            var company = new CompanyDetailsDTO { Id = Guid.NewGuid(), Name="Test"};

            repository.Setup(r => r.GetCompanyAsync(company.Id)).ReturnsAsync(company);
            var controller = new CompanyRepositoryController(repository.Object);

            var actual = await controller.Get(company.Id, true);

            var actionResult = Assert.IsType<ActionResult<CompanyDetailsDTO>>(actual);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var actualCompany = Assert.IsType<CompanyDetailsDTO>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(company, actualCompany);
        }

        [Fact]
        public async Task Get_given_id_that_does_not_exist_returns_404()
        {
            var id = Guid.NewGuid();
            repository.Setup(r => r.GetCompanyAsync(id)).ThrowsAsync(new ArgumentException());
            var controller = new CompanyRepositoryController(repository.Object);

            var actual = await controller.Get(id, true);

            var actionResult = Assert.IsType<ActionResult<CompanyDetailsDTO>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(404, code.StatusCode);
        }

        [Fact]
        public async Task Get_given_id_returns_500_on_internal_error()
        {
            var id = Guid.NewGuid();
            repository.Setup(r => r.GetCompanyAsync(id)).ThrowsAsync(new Exception());
            var controller = new CompanyRepositoryController(repository.Object);

            var actual = await controller.Get(id, true);

            var actionResult = Assert.IsType<ActionResult<CompanyDetailsDTO>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(500, code.StatusCode);
        }

        [Fact]
        public async Task Create_returns_200_and_id_of_created_company()
        {
            var nextMockedId = Guid.NewGuid();
            var company = new CreateCompanyDTO();
            repository.Setup(r => r.CreateCompanyAsync(company)).ReturnsAsync(nextMockedId);
            var controller = new CompanyRepositoryController(repository.Object);

            var actual = await controller.Create(company, true);

            var actionResult = Assert.IsType<ActionResult<Guid>>(actual);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var actualId = Assert.IsType<Guid>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(nextMockedId, actualId);
        }

        [Fact]
        public async Task Create_returns_500_on_internal_error()
        {
            var company = new CreateCompanyDTO();
            repository.Setup(r => r.CreateCompanyAsync(company)).ThrowsAsync(new Exception());
            var controller = new CompanyRepositoryController(repository.Object);

            var actual = await controller.Create(company, true);

            var actionResult = Assert.IsType<ActionResult<Guid>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(500, code.StatusCode);
        }

        [Fact]
        public async Task Delete_returns_200_and_true()
        {
            var id = Guid.NewGuid();
            repository.Setup(r => r.DeleteCompanyAsync(id)).ReturnsAsync(true);
            var controller = new CompanyRepositoryController(repository.Object);

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
            repository.Setup(r => r.DeleteCompanyAsync(id)).ThrowsAsync(new ArgumentException());
            var controller = new CompanyRepositoryController(repository.Object);

            var actual = await controller.Delete(id, true);

            var actionResult = Assert.IsType<ActionResult<bool>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(404, code.StatusCode);
        }

        [Fact]
        public async Task Delete_returns_500_on_internal_error()
        {
            var id = Guid.NewGuid();
            repository.Setup(r => r.DeleteCompanyAsync(id)).ThrowsAsync(new Exception());
            var controller = new CompanyRepositoryController(repository.Object);

            var actual = await controller.Delete(id, true);

            var actionResult = Assert.IsType<ActionResult<bool>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(500, code.StatusCode);
        }

        [Fact]
        public async Task Update_returns_200_and_true()
        {
            var company = new UpdateCompanyDTO { Name = "New Name" };
            repository.Setup(r => r.UpdateCompanyAsync(company)).ReturnsAsync(true);
            var controller = new CompanyRepositoryController(repository.Object);

            var actual = await controller.Update(company, true);

            var actionResult = Assert.IsType<ActionResult<bool>>(actual);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var hasBeenUpdated = Assert.IsType<bool>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.True(hasBeenUpdated);
        }

        [Fact]
        public async Task Update_returns_404_on_not_found()
        {
            var company = new UpdateCompanyDTO { Id = Guid.NewGuid() };
            repository.Setup(r => r.UpdateCompanyAsync(company)).ThrowsAsync(new ArgumentException());
            var controller = new CompanyRepositoryController(repository.Object);

            var actual = await controller.Update(company, true);

            var actionResult = Assert.IsType<ActionResult<bool>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(404, code.StatusCode);
        }

        [Fact]
        public async Task Update_returns_500_on_internal_error()
        {
            var company = new UpdateCompanyDTO { Name = "Updated Name" };
            repository.Setup(r => r.UpdateCompanyAsync(company)).ThrowsAsync(new Exception());
            var controller = new CompanyRepositoryController(repository.Object);

            var actual = await controller.Update(company, true);

            var actionResult = Assert.IsType<ActionResult<bool>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(500, code.StatusCode);
        }
    }
}
