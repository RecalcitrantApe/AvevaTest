using EmployeeApi.Application.Common.Interfaces;
using EmployeeApi.Application.Employees.Commands;
using EmployeeApi.Application.Employees.Queries;
using EmployeeApi.Application.Exceptions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeApi.Web.Controllers
{

    //[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/v1/employees")]
    [Authorize]
    public class EmployeeController : ApiControllerBase
    {
        private readonly ICurrentUserService currentUserService;
        public EmployeeController(ICurrentUserService currentUserService)
        {
            this.currentUserService = currentUserService;
        }

        
        [HttpGet]

        public async Task<ActionResult<IList<EmployeeDto>>> GetEmployees([FromQuery] SearchEmployeeQuery query)
        {
            var user = this.currentUserService.UserId;
            var employees = await Mediator.Send(query);
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            try
            {
                var employees = await Mediator.Send(new GetEmployeeQuery(id));
                return Ok(employees);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }


        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateEmployeeCommand command)
        {
            return await Mediator.Send(command);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateEmployeeCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            try
            {
                await Mediator.Send(command);

                return NoContent();
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }


        [HttpPatch("{employeeId}")]
        public async Task<IActionResult> PartialUpdate(int employeeId, [FromBody] JsonPatchDocument<EmployeeDto> patch)
        {
            try
            {
                var command = new PatchEmployeeCommand { Id = employeeId, Patch = patch };
                await Mediator.Send(command);

                return NoContent();
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
