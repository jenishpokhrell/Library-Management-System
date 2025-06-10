using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string token { get; set; }
        public UsersDto users { get; set; }
    }
}
