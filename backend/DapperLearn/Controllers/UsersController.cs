using DapperLearn.DTOs.Auth;
using DapperLearn.Helper;
using DapperLearn.Interfaces.IRepositories;
using DapperLearn.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly IUserRepository _userRepository;
        private readonly GenerateJWTToken _generateToken;

        public UsersController(IUserServices userServices, IUserRepository userRepository, GenerateJWTToken generateToken)
        {
            _userServices = userServices;
            _generateToken = generateToken;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerDto)
        {
            var registerUser = await _userServices.RegisterUserAsync(registerDto);
            return Ok(registerUser);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var user = await _userServices.LoginAsync(loginDto);
                return Ok(user);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid credentials");
            }
        }

        [HttpPost]
        [Route("Me")]
        public async Task<IActionResult> Me([FromBody] MeDto meDto)
        {
            try
            {
                var me = await _userServices.MeAsync(meDto);
                if (me is null)
                {
                    return Unauthorized("Invalid Token");
                }
                else
                {
                    return Ok(me);
                }
            }
            catch (Exception)
            {
                return Unauthorized("Invalid Token");
            }
        }

        [HttpGet]
        [Route("GetUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userServices.GetUsersAsync();
            if(users is null)
            {
                return NotFound("Users Not Found");
            }

            return Ok(users);
        }

        [HttpGet]
        [Route("GetUsers/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userServices.GetUserByEmail(email);
            if (user is null)
            {
                return NotFound("No User Found Under This Email");
            }
            return Ok(user);
        }

        [HttpGet]
        [Route("GetUsersBy/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var user = await _userServices.GetUserById(User, userId);
            if (user is null)
            {
                return NotFound("No User Found Under This Email");
            }
            return Ok(user);
        }

        [HttpPut]
        [Route("UpdateUser/{userId}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUserDto, int userId)
        {
            var response = await _userServices.UpdateUserAsync(User, updateUserDto, userId);
            if (response.IsSuccess)
            {
                var updatedUser = await _userRepository.GetUserById(userId);
                var newToken = _generateToken.CreateToken(updatedUser);

                Response.Headers.Add("X-New-JWT", newToken);
                return Ok(response);
            }

            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpDelete]
        [Route("DeleteUser/{userId}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var deleteUser = await _userServices.DeleteUserAsync(User, userId);
            if (deleteUser.IsSuccess)
            {
                return StatusCode(deleteUser.StatusCode, deleteUser.Message);
            }
            
            return Ok(deleteUser);
        }
    }
}
