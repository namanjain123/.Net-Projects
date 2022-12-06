using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctionRestApi
{
    class Model
    {
    }
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Salary { get; set; }
        public string Department { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; }
    }
    public class TaskCreateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Salary { get; set; }
        public string Department { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; }
    }
    public class TaskUpdateModel
    {
        public string Name { get; set; }
        public int Salary { get; set; }
        public string Department { get; set; }
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; }
    }


}
