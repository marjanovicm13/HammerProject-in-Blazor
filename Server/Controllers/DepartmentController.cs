using HammerProject.Client.Pages;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System.Configuration;
using System.Data;

namespace HammerProject.Server.Controllers
{
    [ApiController]
    [Route("[controller]")] 
    public class DepartmentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HammerProjectContext _context;
        public DepartmentController(HammerProjectContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpGet]
        [Route("getDepartments")]
        public async Task<List<department>> Get()
        {
            var departments = await _context.department.ToListAsync();
            return departments;
        }


        [HttpPost]
        public IActionResult Post([FromBody] department dep)
        {
            try
            {
                if (dep is null)
                {
                    return BadRequest("Owner object is null");
                }

                _context.department.Add(dep);
                _context.SaveChanges();
                return Ok("200");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] DepartmentUpdate dep)
        {
            try
            {
                if (dep is null)
                {
                    return BadRequest("Owner object is null");
                }
                department departmentToUpdate = _context.department.Find(dep.departmentNo);
                departmentToUpdate.departmentLocation = dep.departmentLocation;
                departmentToUpdate.departmentName = dep.departmentName;
                _context.SaveChanges();
                return Ok("200");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var departmentToDelete = _context.department.Find(id);
                if (departmentToDelete != null)
                {
                    _context.department.Remove(departmentToDelete);
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
