using DapperLearn.DTOs;
using DapperLearn.DTOs.Auth;
using DapperLearn.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<Users>> GetUsers();

        Task RegisterUser(Users users);

        Task<Users> GetUserByEmail(string email);

        Task<Users> GetUserById(int id);

        Task<Users> GetAdmin();

        Task UpdateUser(UpdateUserDto updateUserDto, int userId);

        Task DeleteUser(int id);
    }
}
