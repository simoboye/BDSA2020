using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BDSA2020.Entities;
using BDSA2020.Models;
using BDSA2020.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BDSA2020.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlacementDescriptionRepositoryController : ControllerBase
    {
        private readonly IPlacementDescriptionRepository repository;

        public PlacementDescriptionRepositoryController(IPlacementDescriptionRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PlacementDescription>>> Get()
        {
            try 
            {
                var descriptions = await repository.GetPlacementDescriptionsAsync();
                return Ok(descriptions.ToList());
            } 
            catch (Exception e)
            {
                Util.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PlacementDescription>> Get(int id)
        {
            try 
            {
                var description = await repository.GetPlacementDescriptionAsync(id);

                return Ok(description);
            }
            catch (ArgumentException e)
            {
                Util.LogError(e);
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception e) 
            {
                Util.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> Create([FromBody] PlacementDescription description)
        {
            try 
            {
                var id = await repository.CreatePlacementDescriptionAsync(description);

                return Ok(id);
            }
            catch (ArgumentException e)
            {
                Util.LogError(e);
                return StatusCode(StatusCodes.Status409Conflict);
            }
            catch (Exception e) 
            {
                Util.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            try 
            {
                var isDeleted = await repository.DeletePlacementDescriptionAsync(id);

                return Ok(isDeleted);
            }
            catch (ArgumentException e)
            {
                Util.LogError(e);
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception e) 
            {
                Util.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Update([FromBody] PlacementDescription description)
        {
            try 
            {
                var isUpdated = await repository.UpdatePlacementDescriptionAsync(description);

                return Ok(isUpdated);
            }
            catch (ArgumentException e)
            {
                Util.LogError(e);
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception e) 
            {
                Util.LogError(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}