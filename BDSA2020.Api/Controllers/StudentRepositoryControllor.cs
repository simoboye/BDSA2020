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
    public class StudentRepositoryController : ControllerBase
    {
        private readonly IStudentRepository repository;

        public StudentRepositoryController(IStudentRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDetailsDTO>>> Get(bool isTest = false)
        {
            try 
            {
                var students = await repository.GetStudentsAsync();
                return Ok(students.ToList());
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
        public async Task<ActionResult<StudentDetailsDTO>> Get(Guid id, bool isTest = false)
        {
            try 
            {
                var student = await repository.GetStudentAsync(id);

                return Ok(student);
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
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateStudentDTO student, bool isTest = false)
        {
            try 
            {
                var id = await repository.CreateStudentAsync(student);

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
                var isDeleted = await repository.DeleteStudentAsync(id);

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
        public async Task<ActionResult<bool>> Update([FromBody] UpdateStudentDTO student, bool isTest = false)
        {
            try 
            {
                var isUpdated = await repository.UpdateStudentAsync(student);

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

        [HttpPatch()]
        [Route("save/{studentId}/{descriptionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Save([FromRoute] Guid studentId, [FromRoute] int descriptionId, bool isTest = false)
        {
            try
            {
                var isSaved = await repository.SavePlacementDescription(studentId, descriptionId);

                return Ok(isSaved);
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


        [HttpPatch()]
        [Route("unsave/{studentId}/{descriptionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> UnSave([FromRoute] Guid studentId, [FromRoute] int descriptionId, bool isTest = false)
        {
            try
            {
                var isUnSaved = await repository.UnSavePlacementDescription(studentId, descriptionId);

                return Ok(isUnSaved);
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