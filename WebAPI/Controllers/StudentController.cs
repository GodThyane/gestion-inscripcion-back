using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StudentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            const string query =
                @"select StudentId, StudentCode, StudentLastName, StudentFirstName from dbo.Student ORDER BY StudentCode ASC";
            var table = new DataTable();
            var sqlDataSource = _configuration.GetConnectionString("SubjectAppCon");
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
        public JsonResult Post(Student student)
        {
            var query = @"insert into dbo.Student
                                    (StudentId,StudentLastName,StudentFirstName)
                                    values
                                    (
                                     '" + student.StudentId + @"'
                                     ,'" + student.StudentLastName + @"'
                                     ,'" + student.StudentFirstName + @"'
                                    )
";

            var table = new DataTable();
            var sqlDataSource = _configuration.GetConnectionString("SubjectAppCon");
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
        public JsonResult Put(int id, Student student)
        {
            var query = @"update dbo.Student set 
                          StudentId = '" + student.StudentId + @"'
                         ,StudentLastName = '" + student.StudentLastName + @"'
                         ,StudentFirstName = '" + student.StudentFirstName + @"'
                          where StudentCode = " + id + @"
        ";

            var table = new DataTable();
            var sqlDataSource = _configuration.GetConnectionString("SubjectAppCon");
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
            var query = @"delete from dbo.Student
                            where StudentCode = " + id + @"
";

            var table = new DataTable();
            var sqlDataSource = _configuration.GetConnectionString("SubjectAppCon");
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