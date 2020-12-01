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
    public class CompanyRepositoryController : ControllerBase
    {
        private readonly ICompanyRepository repository;

        public CompanyRepositoryController(ICompanyRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CompanyDetailsDTO>>> Get(bool isTest = false)
        {
            try 
            {
                var companies = await repository.GetCompaniesAsync();
                return Ok(companies.ToList());
            } 
            catch (Exception e)
            {
                Util.LogError(e, isTest);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CompanyDetailsDTO>> Get(Guid id, bool isTest = false)
        {
            try 
            {
                var company = await repository.GetCompanyAsync(id);

                return Ok(company);
            }
            catch (ArgumentException e)
            {
                Util.LogError(e, isTest);
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception e) 
            {
                Util.LogError(e, isTest);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateCompanyDTO company, bool isTest = false)
        {
            try 
            {
                var id = await repository.CreateCompanyAsync(company);

                return Ok(id);
            }
            catch (ArgumentException e)
            {
                Util.LogError(e, isTest);
                return StatusCode(StatusCodes.Status409Conflict);
            }
            catch (Exception e) 
            {
                Util.LogError(e, isTest);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete()]
        [Route("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Delete([FromRoute] Guid id, bool isTest = false)
        {
            try 
            {
                var isDeleted = await repository.DeleteCompanyAsync(id);

                return Ok(isDeleted);
            }
            catch (ArgumentException e)
            {
                Util.LogError(e, isTest);
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception e) 
            {
                Util.LogError(e, isTest);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Update([FromBody] UpdateCompanyDTO company, bool isTest = false)
        {
            try 
            {
                var isUpdated = await repository.UpdateCompanyAsync(company);

                return Ok(isUpdated);
            }
            catch (ArgumentException e)
            {
                Util.LogError(e, isTest);
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception e) 
            {
                Util.LogError(e, isTest);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
