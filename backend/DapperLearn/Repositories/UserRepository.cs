using Dapper;
using DapperLearn.Context;
using DapperLearn.DTOs;
using DapperLearn.DTOs.Auth;
using DapperLearn.Entities;
using DapperLearn.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<Users> GetUserByEmail(string email)
        {
            var query = "SELECT * FROM Users WHERE email = @email";

            using(var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Users>(query, new { email });
            }
        }

        public async Task<IEnumerable<Users>> GetUsers(string role)
        {
            var query = "SELECT * FROM Users WHERE role = @role";

            using(var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Users>(query, new { role = "Member" } );
            }
        }

        public async Task<Users> GetAdmin()
        {
            var query = "SELECT * FROM Users WHERE role = 'Admin'";

            using(var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Users>(query);
            }
        }

        public async Task RegisterUser(Users users)
        {
            var query = "INSERT INTO Users (name, age, address, email, role, Password) VALUES (@name, @age, @address, @email, " +
                "@role, @Password)";

            using(var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, users);
            }
        }

        public async Task<Users> GetUserById(int userId)
        {
            var query = "SELECT * FROM Users WHERE userId = @userId";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Users>(query, new { userId });
            }
        }

        public async Task UpdateUser(UpdateUserDto updateUserDto, int userId)
        {
            var query = "UPDATE Users SET name = @name, age = @age, address = @address, email = @email WHERE userId = @UserId";

            var parameters = new DynamicParameters();
            parameters.Add("userId", userId, DbType.Int32);
            parameters.Add("name", updateUserDto.name, DbType.String);
            parameters.Add("age", updateUserDto.age, DbType.Int32);
            parameters.Add("address", updateUserDto.address, DbType.String);
            parameters.Add("email", updateUserDto.email, DbType.String);

            using(var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task DeleteUser(int userId)
        {
            var query = "DELETE FROM Users WHERE userId = @userId";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { userId } );
            }
        }
    }
}
