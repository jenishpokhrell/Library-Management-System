using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.DTOs.Auth
{
    public class RegisterUserDto
    {
        public string name { get; set; }
        public int age { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public string Password { get; set; }
    }
}
