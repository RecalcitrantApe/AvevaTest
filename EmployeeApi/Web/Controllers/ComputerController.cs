//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;

//namespace EmployeeApi.Web.Controllers
//{
//    [Route("v1/api/employees/{employeeId}/computers")]
//    [ApiController]
//    public class ComputerController : ApiControllerBase
//    {

//        private ILogger<ComputerController> logger;
//        public ComputerController(ILogger<ComputerController> logger)
//        {
//            this.logger = logger;
//        }


//        //[HttpGet]
//        //public ActionResult<ComputerDto> ComputersForEmployee(int employeeId)
//        //{
//        //    try
//        //    {
//        //        //throw new ArgumentException("test");
//        //        logger.LogInformation("Someone requested computers for employee!");
//        //        var employee = EmployeeDataStore.GetAllEmployees().FirstOrDefault(x => x.Id == employeeId);
//        //        if (employee == null)
//        //            return NotFound();

//        //        return Ok(employee.Computers ?? new List<ComputerDto>());
//        //    }
//        //    catch (Exception ee)
//        //    {
//        //        this.logger.LogCritical("Cannot get computers for employee", ee);
//        //        return StatusCode(StatusCodes.Status500InternalServerError);
//        //    }
//        //}

//        //[HttpGet("{computerId}", Name = "GetComputer")]
//        //public IActionResult ComputerForEmployee(int employeeId, int computerId)
//        //{
//        //    var employee = EmployeeDataStore.GetAllEmployees().FirstOrDefault(x => x.Id == employeeId);
//        //    if (employee == null)
//        //        return NotFound();

//        //    var computer = employee.Computers.FirstOrDefault(x => x.Id == computerId);

//        //    if (computer == null)
//        //        return NotFound();

//        //    return Ok(computer);
//        //}

//        //[HttpPost]
//        //public IActionResult CreateComputer(int employeeId, [FromBody] ComputerCreationDto computer)
//        //{

//        //    //if (computer == null)
//        //    //    return BadRequest(); //ApiController does this automatically

//        //    if (!ModelState.IsValid)
//        //    { // re-render the view when validation failed.
//        //        return BadRequest();
//        //    }


//        //    var employee = EmployeeDataStore.GetAllEmployees().FirstOrDefault(x => x.Id == employeeId);
//        //    if (employee == null)
//        //        return NotFound();

//        //    var maxComputerId = EmployeeDataStore.GetAllEmployees().SelectMany(x => x.Computers).Max(x => x.Id);

//        //    var computerDto = new ComputerDto
//        //    {
//        //        Id = maxComputerId + 1,
//        //        Name = computer.Name
//        //    };
//        //    employee.AddComputer(computerDto);

//        //    return CreatedAtRoute("GetComputer", new { employeeId = employeeId, computerId = computerDto.Id }, computerDto);
//        //}

//        //[HttpPut("{computerId}")]
//        //public IActionResult UpdateComputer(int employeeId, int computerId, [FromBody] ComputerUpdateDto dto)
//        //{
//        //    //if (computer == null)
//        //    //    return BadRequest(); //ApiController does this automatically

//        //    if (!ModelState.IsValid)
//        //    { // re-render the view when validation failed.
//        //        return BadRequest();
//        //    }

//        //    var employee = EmployeeDataStore.GetAllEmployees().FirstOrDefault(x => x.Id == employeeId);
//        //    if (employee == null)
//        //        return NotFound();

//        //    var computer = employee.Computers.FirstOrDefault(x => x.Id == computerId);

//        //    if (computer == null)
//        //        return NotFound();

//        //    employee.UpdateComputer(dto, computerId);
//        //    return NoContent();

//        //}

//        //[HttpPatch("{computerId}")]
//        //public IActionResult PartialUpdate(int employeeId, int computerId, [FromBody] JsonPatchDocument<ComputerUpdateDto> patch)
//        //{
//        //    var employee = EmployeeDataStore.GetAllEmployees().FirstOrDefault(x => x.Id == employeeId);
//        //    if (employee == null)
//        //        return NotFound();

//        //    var computer = employee.Computers.FirstOrDefault(x => x.Id == computerId);

//        //    var computerToPatch = new ComputerUpdateDto
//        //    {
//        //        Name = computer.Name,
//        //        Description = computer.Description
//        //    };

//        //    patch.ApplyTo(computerToPatch, ModelState);

//        //    ComputerUpdateValidator validator = new ComputerUpdateValidator();

//        //    ValidationResult result = validator.Validate(computerToPatch);

//        //    if (!result.IsValid)
//        //    { // re-render the view when validation failed.
//        //        return BadRequest(result);
//        //    }

//        //    employee.UpdateComputer(computerToPatch, computerId);
//        //    return NoContent();
//        //}

//        //[HttpDelete("{computerId}")]
//        //public IActionResult DeleteComputer(int employeeId, int computerId)
//        //{
//        //    var employee = EmployeeDataStore.GetAllEmployees().FirstOrDefault(x => x.Id == employeeId);
//        //    if (employee == null)
//        //        return NotFound();

//        //    var computer = employee.Computers.FirstOrDefault(x => x.Id == computerId);

//        //    if (computer == null)
//        //        return NotFound();
//        //    employee.DeleteComputer(computerId);
//        //    return NoContent();

//        //}

//    }
//}
