﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.DTOs
{
    public class UsersDto
    {
        public int userId { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string role { get; set; }
    }
}
