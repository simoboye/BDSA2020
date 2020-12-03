using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BDSA2020.Models;
using BDSA2020.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BDSA2020.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KeywordRepositoryController : ControllerBase
    {
        private readonly IKeywordRepository repository;

        public KeywordRepositoryController(IKeywordRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<string>>> Get(bool isTest = false)
        {
            try 
            {
                var companies = await repository.GetKeywordsAsync();
                return Ok(companies.ToList());
            } 
            catch (Exception e)
            {
                Util.LogError(e, isTest);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
