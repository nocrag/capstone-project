using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MJRecords.Model;
using MJRecords.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MJRecords.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }


        // GET: api/<EmployeeController>
        /// <summary>
        /// Retrieves a list of employees matching the specified search criteria.
        /// </summary>
        /// <param name="searchDTO">The search parameters to filter employees.</param>
        /// <returns>A list of employees matching the search criteria if authorized.</returns>
        /// <response code="200">Returns the list of matching employees.</response>
        /// <response code="401">Unauthorized access.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<EmployeeSearchResultDTO>>> Get([FromQuery] EmployeeSearchDTO searchDTO)
        {
            List<EmployeeSearchResultDTO> results = await _employeeService.SearchEmployeeAsync(searchDTO);
            return Ok(results);
        }

        // GET api/<EmployeeController>/5
        /// <summary>
        /// Retrieves detailed information for a specific employee by their ID.
        /// </summary>
        /// <param name="id">The unique identifier of the employee.</param>
        /// <returns>The detailed employee information if found and authorized.</returns>
        /// <response code="200">Returns the detailed employee information.</response>
        /// <response code="400">The provided employee ID is invalid.</response>
        /// <response code="404">No employee found with the specified ID.</response>
        /// <response code="401">Unauthorized access.</response>
        [HttpGet("Detail/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<EmployeeSearchResultDetailedDTO>> Detail(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            EmployeeSearchDTO searchDTO = new();
            searchDTO.EmployeeId = id;
            var result = await _employeeService.SearchEmployeeDetailedAsync(searchDTO);
            EmployeeSearchResultDetailedDTO resultDTO = result.FirstOrDefault();
            if (resultDTO != null)
            {
                return Ok(resultDTO);
            }
            return NotFound();
        }

        // POST api/<EmployeeController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/<EmployeeController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<EmployeeController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
