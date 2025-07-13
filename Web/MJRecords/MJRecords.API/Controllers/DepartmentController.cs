using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MJRecords.Model;
using MJRecords.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MJRecords.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: api/<DepartmentAPIController>
        /// <summary>
        /// Retrieves a list of all departments.
        /// </summary>
        /// <returns>A list of departments if the request is authorized.</returns>
        /// <response code="200">Returns the list of departments.</response>
        /// <response code="401">Unauthorized access.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<DepartmentDTO>>> Get()
        {
            List<DepartmentDTO>? departments = await _departmentService.GetAllAsync();
            if(departments == null || departments.Count == 0)
            {
                return new List<DepartmentDTO>();
            }

            return Ok(departments.Where(d => d.InvocationDate <= DateTime.Today).OrderBy(d => d.Id).ToList());
        }

        // GET api/<DepartmentAPIController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<DepartmentAPIController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/<DepartmentAPIController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<DepartmentAPIController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
