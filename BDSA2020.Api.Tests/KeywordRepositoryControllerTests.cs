using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BDSA2020.Api.Controllers;
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
    public class KeywordRepositoryControllerTests
    {
        private readonly Mock<IKeywordRepository> repository;

        public KeywordRepositoryControllerTests()
        {
            repository = new Mock<IKeywordRepository>();
        }

        [Fact]
        public async Task Get_returns_200_and_company()
        {
            var keywords = new [] 
            {
                "Testing",
                "C#",
                "Java",
                "FullStack",
                "Frontend",
                "Backend",
                "UML",
                "DevOps",
                "Communication",
                "JavaScript"
            };

            repository.Setup(r => r.GetKeywordsAsync()).ReturnsAsync(keywords);
            var controller = new KeywordRepositoryController(repository.Object);

            var actual = await controller.Get(true);

            var actionResult = Assert.IsType<ActionResult<IEnumerable<string>>>(actual);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var actualCompanies = Assert.IsType<List<string>>(okResult.Value);

            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(keywords, actualCompanies);
        }

        [Fact]
        public async Task Get_returns_500_on_internal_error()
        {
            repository.Setup(r => r.GetKeywordsAsync()).ThrowsAsync(new Exception());
            var controller = new KeywordRepositoryController(repository.Object);

            var actual = await controller.Get(true);

            var actionResult = Assert.IsType<ActionResult<IEnumerable<string>>>(actual);
            var code = Assert.IsType<StatusCodeResult>(actionResult.Result);
            Assert.Equal(500, code.StatusCode);
        }
    }
}
