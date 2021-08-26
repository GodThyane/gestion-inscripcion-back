using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            const string query = @"select DepartmentId, DepartmentName from dbo.Department ORDER BY DepartmentId ASC";
            var table = new DataTable();
            var sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            using (var myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (var myCommand = new SqlCommand(query, myCon))
                {
                    var myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Department department)
        {
            var query = @"insert into dbo.Department values
                                    ('" + department.DepartmentName + @"')";

            var table = new DataTable();
            var sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            using (var myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (var myCommand = new SqlCommand(query, myCon))
                {
                    var myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myCon.Close();
                }
            }

            return new JsonResult("Agregado satisfactoriamente");
        }

        [HttpPut("{id}")]
        public JsonResult Put(int id, Department department)
        {
            var query = @"update dbo.Department set 
                          DepartmentName = '" + department.DepartmentName + @"'
                          where DepartmentId = " + id + @"
        ";

            var table = new DataTable();
            var sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            using (var myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (var myCommand = new SqlCommand(query, myCon))
                {
                    var myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myCon.Close();
                }
            }

            return new JsonResult("Modificado satisfactoriamente");
        }
        
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            var query = @"delete from dbo.Department
                            where DepartmentId = " + id + @"
";

            var table = new DataTable();
            var sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            using (var myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (var myCommand = new SqlCommand(query, myCon))
                {
                    var myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myCon.Close();
                }
            }

            return new JsonResult("Eliminado satisfactoriamente");
        }
    }
}