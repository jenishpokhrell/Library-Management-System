using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.DTOs.Auth
{
    public class LoginDto
    {
        public string email { get; set; }
        public string Password { get; set; }
    }
}
