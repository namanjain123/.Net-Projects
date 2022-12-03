using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDbCrud.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDbCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        //setting up the configuration dependency
        private readonly IConfiguration _configuration;
        //Setting Up the clinet of mongo db
        
        public DepartmentController(IConfiguration configuration)
        {
            //Initializing the MongoDbClinet with its Connection to be used
            
            //Dependency Injection of the configuration file
            _configuration = configuration;
        }
        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClinet = new MongoClient(_configuration.GetConnectionString("EmployeeAppConnection"));
            //Getting The Data from the Database
            var dbList = dbClinet.GetDatabase("testdb").GetCollection<Department>("Department").AsQueryable();
            return new JsonResult(dbList);
        }
        [HttpGet("{id}")]
        public JsonResult GetById(int id)
        {
            MongoClient dbClinet = new MongoClient(_configuration.GetConnectionString("EmployeeAppConnection"));
            var filter = Builders<Department>.Filter.Eq("Departmentid", id);
            var dbList = dbClinet.GetDatabase("testdb").GetCollection<Department>("Department").Find<Department>(filter).FirstOrDefault();
            return new JsonResult(dbList);
        }
        [HttpPost]
        public JsonResult Post(Department dep)
        {
            MongoClient dbClinet = new MongoClient(_configuration.GetConnectionString("EmployeeAppConnection"));
            int gotDepartmentId = dbClinet.GetDatabase("testdb").GetCollection<Department>("Department").AsQueryable().Count();
            dep.Departmentid = gotDepartmentId + 1;
            dbClinet.GetDatabase("testdb").GetCollection<Department>("Department").InsertOne(dep);
            return new JsonResult("adde data", dep);
        }
        [HttpPut]
        public JsonResult Updatebyid(Department dep)
        {
            MongoClient dbClinet = new MongoClient(_configuration.GetConnectionString("EmployeeAppConnection"));
            var filter = Builders<Department>.Filter.Eq("Departmentid", dep.Departmentid);
            var updateddata = Builders<Department>.Update.Set("Departmentname", dep.Departmentname);
            dbClinet.GetDatabase("testdb").GetCollection<Department>("Department").UpdateOne(filter, updateddata);
            return new JsonResult($"result is update for {dep.Departmentid}");

        }
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClinet = new MongoClient(_configuration.GetConnectionString("EmployeeAppConnection"));
            var filter = Builders<Department>.Filter.Eq("Departmentid", id);
            dbClinet.GetDatabase("testdb").GetCollection<Department>("Department").DeleteOne(filter);
            return new JsonResult($"Your data is deleted for this id :{id}");
        }
    }
}
