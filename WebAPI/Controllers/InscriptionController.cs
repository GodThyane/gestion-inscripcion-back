using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscriptionController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public InscriptionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            const string query =
                @"select st.StudentCode ,st.StudentFirstName, st.StudentLastName,sb.SubjectCode, sb.SubjectName, sb.SubjectInitHour, sb.SubjectFinishHour from dbo.Inscription i, dbo.Student st, dbo.Subject sb
                    WHERE i.StudentCode = st.StudentCode
                    AND i.SubjectCode = sb.SubjectCode";
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
        
        [HttpGet("subject/{id}")]
        public JsonResult GetSubjectNotInsc(int id)
        {
            var query = @"select sb.SubjectCode, sb.SubjectName, sb.SubjectDescription, sb.SubjectInitHour, sb.SubjectFinishHour
                    from dbo.Subject sb
                    where not exists (select 1 from dbo.Inscription i
					                     where I.StudentCode = " + id + @"
					                    AND i.SubjectCode = sb.SubjectCode)";
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
        
        [HttpGet("student/{id}")]
        public JsonResult GetStudentNotInsc(int id)
        {
            var query = @"select StudentId, StudentCode, StudentLastName, StudentFirstName
                            from dbo.Student st
                            where not exists (select 1 from dbo.Inscription i
					                            where I.SubjectCode = " + id + @"
					                            AND i.StudentCode = st.StudentCode)";
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
        public JsonResult Post(Inscription inscription)
        {
            var query = @"insert into dbo.Inscription
                                    (StudentCode,SubjectCode)
                                    values
                                    (
                                     '" + inscription.StudentCode + @"'
                                     ,'" + inscription.SubjectCode + @"'
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

        [HttpDelete]
        public JsonResult Delete(Inscription inscription)
        {
            var query = @"delete from dbo.Inscription
                            where StudentCode = " + inscription.StudentCode + @"
                            and SubjectCode = " + inscription.SubjectCode + @"
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