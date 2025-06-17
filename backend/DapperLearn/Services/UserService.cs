using AutoMapper;
using DapperLearn.DTOs;
using DapperLearn.DTOs.Auth;
using DapperLearn.DTOs.Response;
using DapperLearn.Entities;
using DapperLearn.Helper;
using DapperLearn.Interfaces.IRepositories;
using DapperLearn.Interfaces.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DapperLearn.Services
{
    public class UserService : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly GenerateJWTToken _generateToken;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IMapper mapper, GenerateJWTToken generateToken, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _generateToken = generateToken;
            _configuration = configuration;
        }

       
        public async Task<UsersDto> GetUserByEmail(string email)
        {
            var findUser = await _userRepository.GetUserByEmail(email);
            
            return _mapper.Map<UsersDto>(findUser);
        }

        public async Task<UsersDto> GetUserById(ClaimsPrincipal User, int userId)
        {
            var loggedInUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            var findUser =  await _userRepository.GetUserById(userId);
            if (loggedInUser != findUser.userId && userRole != "Admin")
            {
                throw new UnauthorizedAccessException("You are not authorized to view this details.");
            }
           
            return _mapper.Map<UsersDto>(findUser);
        }

        public async Task<IEnumerable<UsersDto>> GetUsersAsync(string role)
        {
            var users = await _userRepository.GetUsers(role);

            
            return _mapper.Map<IEnumerable<UsersDto>>(users);
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmail(loginDto.email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid Credentials");
            }

            var newToken = _generateToken.CreateToken(user);
            var userInfo = GenerateUserInfo(user);

            return new LoginResponseDto()
            {
                token = newToken,
                users = userInfo
            };
        }

        public async Task<LoginResponseDto> MeAsync(MeDto meDto)
        {
            ClaimsPrincipal handler = new JwtSecurityTokenHandler().ValidateToken(meDto.token, new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                ValidAudience = _configuration["JWT:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]))
            }, out SecurityToken securityToken);

            string decodedEmail = handler.Claims.First(q => q.Type == ClaimTypes.Email).Value;

            if(decodedEmail is null)
            {
                return null;
            }

            var user = await _userRepository.GetUserByEmail(decodedEmail);

            var newToken = _generateToken.CreateToken(user);
            var userInfo = GenerateUserInfo(user);

            return new LoginResponseDto()
            {
                token = newToken,
                users = userInfo
            };
            
        }

        public async Task<GeneralResponseDto> RegisterUserAsync(RegisterUserDto registerDto)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var userExists = await _userRepository.GetUserByEmail(registerDto.email);

            if (userExists is not null)
            {
                return new GeneralResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "User already exists"
                };
            }

            var adminExists = await _userRepository.GetAdmin();

            if(registerDto.role == "Admin" && adminExists is not null)
            {
                return new GeneralResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Admin already exists."
                };
            }

            var newUser = new Users
            {
                name = registerDto.name,
                age = registerDto.age,
                address = registerDto.address,
                email = registerDto.email,
                role = registerDto.role,
                Password = hashedPassword
            };

            if (registerDto.role != "Member" && registerDto.role != "Admin")
            {
                return new GeneralResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Enter valid role: 'Member'."
                };
            }

            await _userRepository.RegisterUser(newUser);

            return new GeneralResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "User registered successfully."
            };
        }

        public async Task<GeneralResponseDto> UpdateUserAsync(ClaimsPrincipal User, UpdateUserDto updateUserDto, int userId)
        {
            var currentUser = User.FindFirst(ClaimTypes.NameIdentifier);


            var user = await _userRepository.GetUserById(userId);

            if (Int32.Parse(currentUser.Value) != user.userId)
            {
                return new GeneralResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "You are not authorized to update this user's data."
                };
            }

            
            await _userRepository.UpdateUser(updateUserDto, userId);


            return new GeneralResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "User updated successfully."
            };
        }

        public async Task<GeneralResponseDto> DeleteUserAsync(ClaimsPrincipal User, int userId)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user is null)
            {
                return new GeneralResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "User doesn't exist."
                };
            }

            var currentUser = User.FindFirst(ClaimTypes.NameIdentifier);

            if(Int32.Parse(currentUser.Value) != user.userId)
            {
                return new GeneralResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "You are not authorized to delete this user's data."
                };
            }

            await _userRepository.DeleteUser(userId);
            return new GeneralResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Profile Removed Successfully."
            };
        }


        private UsersDto GenerateUserInfo(Users users)
        {
            var userInfo = _mapper.Map<UsersDto>(users);
            return userInfo;
        }
    }
}
