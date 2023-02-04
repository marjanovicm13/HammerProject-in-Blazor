using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace HammerProject.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : Controller
    {
        private readonly HammerProjectContext _context;
        private readonly IConfiguration _configuration;
        public EmployeeController(HammerProjectContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("getEmployees")]
        public async Task<List<employee>> Get()
        {
            var employees = await _context.employee.ToListAsync();
            return employees;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] employee emp)
        {
            try
            {
                if (emp is null)
                {
                    return BadRequest("Owner object is null");
                }

                _context.employee.Add(emp);
                _context.SaveChanges();
                return Ok("200");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpPut]
        public IActionResult Put([FromBody] EmployeeUpdate emp)
        {
            try
            {
                if (emp is null)
                {
                    return BadRequest("Owner object is null");
                }
                employee employeeToUpdate = _context.employee.Find(emp.employeeNo);
                employeeToUpdate.employeeName = emp.employeeName;
                employeeToUpdate.salary = emp.salary;
                employeeToUpdate.departmentNo = emp.departmentNo;
                _context.SaveChanges();
                return Ok("200");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var employeeToDelete = _context.employee.Find(id);
                if (employeeToDelete != null)
                {
                    _context.employee.Remove(employeeToDelete);
                    _context.SaveChanges();
                    return Ok("200");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error " + ex);
            }
        }
    }
}
