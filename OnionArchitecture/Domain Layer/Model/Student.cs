﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Model
{
    public class Student :Base
    {
        [Key]
        public int Id { get;set; }
        public string? Name { get;set; }
        public string? Email { get;set; }
        public int? Age { get; set; }
    }
}
