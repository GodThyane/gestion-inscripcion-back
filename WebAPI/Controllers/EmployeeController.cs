using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            const string query = @"select EmployeeId, EmployeeName, Department,
            convert(varchar(10),DateOfJoining,120) as DateOfJoining, PhotoFileName 
            from dbo.Employee";

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
        public JsonResult Post(Employee employee)
        {
            var query = @"insert into dbo.Employee
                                    (EmployeeName,Department,DateOfJoining,PhotoFileName)
                                    values
                                    (
                                     '" + employee.EmployeeName + @"'
                                     ,'" + employee.Department + @"'
                                     ,'" + employee.DateOfJoining + @"'
                                     ,'" + employee.PhotoFileName + @"'
                                    )
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

            return new JsonResult("Agregado satisfactoriamente");
        }

        [HttpPut("{id}")]
        public JsonResult Put(int id, Employee employee)
        {
            var query = @"update dbo.Employee set 
                          EmployeeName = '" + employee.EmployeeName + @"'
                         ,Department = '" + employee.Department + @"'
                         ,DateOfJoining = '" + employee.DateOfJoining + @"'
                         ,PhotoFileName = '" + employee.PhotoFileName + @"'
                          where EmployeeId = " + id + @"
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
            var query = @"delete from dbo.Employee
                            where EmployeeId = " + id + @"
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

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                var filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }
    }
}