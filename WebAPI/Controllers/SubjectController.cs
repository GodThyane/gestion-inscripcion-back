using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebAPI.Models;
using static System.Int32;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SubjectController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            const string query =
                @"select SubjectCode, SubjectName, SubjectDescription, SubjectInitHour, SubjectFinishHour from dbo.Subject ORDER BY SubjectCode ASC";
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
        public JsonResult Post(Subject subject)
        {
            Console.WriteLine(subject.SubjectFinishHour);
            var sInit = subject.SubjectInitHour.Split(":");
            var dateInit = new DateTime(1999, 1, 1, Parse(sInit[0]), Parse(sInit[1]), 0);
            
            var sFinish = subject.SubjectFinishHour.Split(":");
            var dateFinish = new DateTime(1999, 1, 1, Parse(sFinish[0]), Parse(sFinish[1]), 0);
            
            
            const string format = "yyyy-MM-dd HH:mm:ss";  
            
            var query = @"insert into dbo.Subject
                                    (SubjectName, SubjectDescription, SubjectInitHour, SubjectFinishHour)
                                    values
                                    (
                                     '" + subject.SubjectName + @"'
                                     ,'" + subject.SubjectDescription + @"'
                                     ,'" + dateInit.ToString(format) + @"'
                                     ,'" + dateFinish.ToString(format) + @"'
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
        public JsonResult Put(int id, Subject subject)
        {
            
            var sInit = subject.SubjectInitHour.Split(":");
            var dateInit = new DateTime(1999, 1, 1, Parse(sInit[0]), Parse(sInit[1]), 0);
            
            var sFinish = subject.SubjectFinishHour.Split(":");
            var dateFinish = new DateTime(1999, 1, 1, Parse(sFinish[0]), Parse(sFinish[1]), 0);
            
            
            const string format = "yyyy-MM-dd HH:mm:ss";  
            
            var query = @"update dbo.Subject set 
                          SubjectName = '" + subject.SubjectName + @"'
                         ,SubjectDescription = '" + subject.SubjectDescription + @"'
                         ,SubjectInitHour = '" + dateInit.ToString(format) + @"'
                         ,SubjectFinishHour = '" + dateFinish.ToString(format) + @"'
                          where SubjectCode = " + id + @"
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
            var query = @"delete from dbo.Subject
                            where SubjectCode = " + id + @"
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