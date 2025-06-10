using DapperLearn.DTOs;
using DapperLearn.DTOs.Auth;
using DapperLearn.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DapperLearn.Interfaces.IServices
{
    public interface IUserServices
    {
        Task<IEnumerable<UsersDto>> GetUsersAsync();

        Task<GeneralResponseDto> RegisterUserAsync(RegisterUserDto registerDto);

        Task<UsersDto> GetUserByEmail(string email);

        Task<UsersDto> GetUserById(ClaimsPrincipal User, int userId);

        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);

        Task<LoginResponseDto> MeAsync(MeDto meDto);

        Task<GeneralResponseDto> UpdateUserAsync(ClaimsPrincipal User, UpdateUserDto updateUserDto, int userId);

        Task<GeneralResponseDto> DeleteUserAsync(ClaimsPrincipal User, int userId);
    }
}
