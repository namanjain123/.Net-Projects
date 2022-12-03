using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDbCrud.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDbCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        //setting up the configuration dependency
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            //Dependency Injection of the configuration file
            _configuration = configuration;
            _env = env;
        }
        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClinet = new MongoClient(_configuration.GetConnectionString("EmployeeAppConnection"));
            //Getting The Data from the Database
            var dbList = dbClinet.GetDatabase("testdb").GetCollection<Employee>("Employee").AsQueryable();
            return new JsonResult(dbList);
        }
        [HttpGet("{id}")]
        public JsonResult GetById(int id)
        {
            MongoClient dbClinet = new MongoClient(_configuration.GetConnectionString("EmployeeAppConnection"));
            var filter = Builders<Employee>.Filter.Eq("Employeeid", id);
            var dbList = dbClinet.GetDatabase("testdb").GetCollection<Employee>("Employee").Find<Employee>(filter).FirstOrDefault();
            return new JsonResult(dbList);
        }

        [HttpPost]
        public JsonResult Post(Employee dep)
        {
            MongoClient dbClinet = new MongoClient(_configuration.GetConnectionString("EmployeeAppConnection"));
            int gotEmployeeId = dbClinet.GetDatabase("testdb").GetCollection<Employee>("Employee").AsQueryable().Count();
            dep.EmployeeId = gotEmployeeId + 1;
            dbClinet.GetDatabase("testdb").GetCollection<Employee>("Employee").InsertOne(dep);
            return new JsonResult("added data", dep);
        }
        [HttpPut]
        public JsonResult Updatebyid(Employee dep)
        {
            MongoClient dbClinet = new MongoClient(_configuration.GetConnectionString("EmployeeAppConnection"));
            var filter = Builders<Employee>.Filter.Eq("Employeeid", dep.EmployeeId);
            var updateddata = Builders<Employee>.Update
                .Set("Employeename", dep.EmployeeName)
                .Set("Department",dep.EmployeeId)
                .Set("DateOfJoining",dep.DateOfJoining)
                .Set("Photo",dep.Photo);
            dbClinet.GetDatabase("testdb").GetCollection<Employee>("Employee").UpdateOne(filter, updateddata);
            return new JsonResult($"result is update for {dep.EmployeeId}");

        }
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClinet = new MongoClient(_configuration.GetConnectionString("EmployeeAppConnection"));
            var filter = Builders<Employee>.Filter.Eq("Employeeid", id);
            dbClinet.GetDatabase("testdb").GetCollection<Employee>("Employee").DeleteOne(filter);
            return new JsonResult($"Your data is deleted for this id :{id}");
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
                var physicalPath = _env.ContentRootPath + "/Photo" + filename;

                using(var stream=new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(filename);

            }
            catch (Exception)
            {
                return new JsonResult("wrongimage.png");
            }
        }
    }
}
