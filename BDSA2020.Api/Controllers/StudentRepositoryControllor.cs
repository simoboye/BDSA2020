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
        public async Task<ActionResult<IEnumerable<Student>>> Get()
        {
            try 
            {
                var students = await repository.GetStudentsAsync();
                return Ok(students.ToList());
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
        public async Task<ActionResult<Student>> Get(int id)
        {
            try 
            {
                var student = await repository.GetStudentAsync(id);

                return Ok(student);
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
        public async Task<ActionResult<int>> Create([FromBody] CreateStudentDTO student)
        {
            try 
            {
                var id = await repository.CreateStudentAsync(student);

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
                var isDeleted = await repository.DeleteStudentAsync(id);

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
        public async Task<ActionResult<bool>> Update([FromBody] Student student)
        {
            try 
            {
                var isUpdated = await repository.UpdateStudentAsync(student);

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