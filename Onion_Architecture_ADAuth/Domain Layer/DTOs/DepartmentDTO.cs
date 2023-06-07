using Domain_Layer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.DTOs
{
    public class DepartmentDTO
    {
        public int Id { get; set; }
        public string DepName { get; set; }
        public Student Students { get; set; }
    }
}
