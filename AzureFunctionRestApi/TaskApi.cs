using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace AzureFunctionRestApi
{
    
    public static class TaskApi
    {
        public static readonly List<Task> Items = new List<Task>();

        //Post
        [FunctionName("CreateTask")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "task")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Creating a new Task List Item");

            string request = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject<TaskCreateModel>(request);
            var task = new Task() { 
                Name = data.Name, 
                Salary = data.Salary,
                Department=data.Department,
                TaskName = data.TaskName,
                TaskDescription=data.TaskDescription };
            task.Id = Items.LastOrDefault().Id+1;
            Items.Add(task);
            return new OkObjectResult(task);
        }

        //Get all data
        [FunctionName("GetAllTasks")]
        public static IActionResult GetAllTasks(
            [HttpTrigger(AuthorizationLevel.Anonymous,"get",Route ="task")]
        HttpRequest req, ILogger log
            )
        {
            log.LogInformation("Get Task List Items");
            return new OkObjectResult(Items);
        }

        //Get By Id
        [FunctionName("GetAllTasksById")]
        public static IActionResult GetAllTasksById(
            [HttpTrigger(AuthorizationLevel.Anonymous,"get",Route ="task/{id}")]
        HttpRequest req, ILogger log,int id
            )
        {
            var task = Items.FirstOrDefault(a => a.Id == id);
            if (task == null)
            {
                return new NotFoundResult();
            }
            log.LogInformation("Get Task List Items");
            return new OkObjectResult(Items);
        }

        //Update the data
        [FunctionName("UpdateTask")]
        public static async Task<IActionResult> UpdateTaskAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous,"put",Route ="task/{id}")]
        HttpRequest req, ILogger log, int id
            )
        {
            var task = Items.FirstOrDefault(a => a.Id == id);
            if (task == null)
            {
                return new NotFoundResult();
            }
            string request = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject<TaskUpdateModel>(request);
            task.IsCompleted = data.IsComplete;           
            if (!string.IsNullOrEmpty(data.Name))
            {
                task.Name = data.Name;
            }
            if (!string.IsNullOrEmpty(data.Department))
            {
                task.Department = data.Department;
            }
            if (!string.IsNullOrEmpty(data.TaskName))
            {
                task.TaskName = data.TaskName;
            }
            if (!string.IsNullOrEmpty(data.TaskDescription))
            {
                task.TaskDescription = data.TaskDescription;
            }
            task.Salary = data.Salary;
            
            log.LogInformation("Get Task List Items");
            return new OkObjectResult(task);
        }

        //Delete the data
        [FunctionName("DeleteTask")]
        public static IActionResult DeleteTask(
            [HttpTrigger(AuthorizationLevel.Anonymous,
                "delete", Route = "task/{id}")]
            HttpRequest req,
            ILogger log, int id)
        {
            var task = Items.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return new NotFoundResult();
            }
            Items.Remove(task);
            return new OkResult();
        }

    }
}
