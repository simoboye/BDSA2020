using System.Collections.Generic;
using System.Threading.Tasks;
using BDSA2020.Entities;
using BDSA2020.Models;
using Microsoft.AspNetCore.Mvc;

namespace BDSA2020.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RepositoryController : ControllerBase
    {
        private readonly IRepository repository;

        public RepositoryController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<Student>> Get()
        {
            return await repository.GetStudentsAsync();
        }
    }
}