using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text.Json.Serialization;

namespace HammerProject.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueryController : Controller
    {
        private readonly HammerProjectContext _context;
        private readonly IConfiguration _configuration;
        public QueryController(IConfiguration configuration, HammerProjectContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        [Route("avgSalary")]
        public JsonResult Get()
        {
                string query = @"SELECT AVG(salary) 'AverageSalary' 
	                      FROM employee, department 
	                      WHERE employee.departmentNo = department.departmentNo && departmentLocation != 'London';";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
                MySqlDataReader myReader;
                using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
                {
                    mycon.Open();
                    using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        mycon.Close();
                    }
                }
              var list = Newtonsoft.Json.JsonConvert.SerializeObject(table);
              return new JsonResult(list);
        }

        [HttpGet]
        [Route("locations")]
        public JsonResult GetLocations()
        {
            string query = @"SELECT departmentLocation, count(employee.departmentNo) as 'NumberOfEmployees' 
	                            FROM department, employee
	                            WHERE employee.departmentNo = department.departmentNo 
	                            GROUP BY departmentLocation 
	                            HAVING count(employee.departmentNo)>1;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            var list = Newtonsoft.Json.JsonConvert.SerializeObject(table);
            return new JsonResult(list);
        }

        [HttpGet]
        [Route("developmentLocations")]
        public JsonResult GetDevelopmentLocations()
        {
            string query = @"SELECT departmentLocation, IF(departmentName = 'development', count(employee.departmentNo), 0) as 'DevelopmentEmployees' 
	                            FROM department, employee
	                            WHERE employee.departmentNo = department.departmentNo 
	                            GROUP BY departmentName, departmentLocation
	                            ;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            var list = Newtonsoft.Json.JsonConvert.SerializeObject(table);
            return new JsonResult(list);
        }

        [HttpGet]
        [Route("secondHighestSalary")]
        public JsonResult GetSecondHighestSalary()
        {
            string query = @"SELECT DISTINCT salary
                                FROM employee 
                                ORDER BY salary DESC
                                LIMIT 1 , 1
	                            ;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            var list = Newtonsoft.Json.JsonConvert.SerializeObject(table);
            return new JsonResult(list);
        }

        [HttpGet]
        [Route("vwDepartment")]
        public JsonResult GetViewVwDepartment()
        {
            string query = @"SELECT departmentNo, CONCAT(departmentName, ' ', departmentLocation) AS 'departmentDescription' FROM department;"; 
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            var list = Newtonsoft.Json.JsonConvert.SerializeObject(table);
            return new JsonResult(list);
        }

        [HttpPost]
        [Route("increaseSalary")]
        public JsonResult IncreaseSalary([FromBody] IncreaseSalary incSalary)
        {
            string query = @"update employee set salary = salary + (salary * (@increasePercentage/100)) where employee.employeeNo = @EmployeeNo;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@employeeNo", incSalary.employeeNo);
                    myCommand.Parameters.AddWithValue("@increasePercentage", incSalary.increasePercentage);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("Salary increased successfully");
        }
        [HttpPost]
        [Route("decreaseSalary")]
        public JsonResult DecreaseSalary([FromBody] IncreaseSalary incSalary)
        {
            string query = @"update employee set salary = salary - (salary * (@decreasePercentage/100)) where employee.employeeNo = @employeeNo;";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@employeeNo", incSalary.employeeNo);
                    myCommand.Parameters.AddWithValue("@decreasePercentage", incSalary.increasePercentage);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("Salary increased successfully");
        }
    }
}
