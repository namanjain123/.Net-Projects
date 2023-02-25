using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Model
{
    public class Department : Base
    {
        public int Id {get;set;}   
        public string DepName { get;set;}
        public Student Students { get; set; }
    
    }
}
