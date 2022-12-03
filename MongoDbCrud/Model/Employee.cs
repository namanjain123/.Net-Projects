using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDbCrud.Model
{
    public class Employee
    {
        public Object Id { get; set; }
        public int EmployeeId{ get; set; }
        public string EmployeeName{ get; set; }
        public string Department { get; set; }
        public string DateOfJoining { get; set; }
        public string Photo{ get; set; }
    }
}