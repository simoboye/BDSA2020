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
            catch (ArgumentException)
            {
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