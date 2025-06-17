using Dapper;
using DapperLearn.Context;
using DapperLearn.DTOs.Auth;
using DapperLearn.DTOs.Books;
using DapperLearn.DTOs.Fine;
using DapperLearn.DTOs.Loan;
using DapperLearn.Entities;
using DapperLearn.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DapperLearn.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly DapperContext _dbo;

        public LoanRepository(DapperContext dbo)
        {
            _dbo = dbo;
        }

        public async Task AddLoanDetails(Loan loan, Fine fine)
        {
            var query1 = "INSERT INTO Loan (bookId, userId, loanDate, returnDate, isReturned) VALUES (@bookId, @userId, @loanDate, " +
                "@returnDate, @isReturned) SELECT CAST(SCOPE_IDENTITY() AS INT)";

            var query2 = "INSERT INTO Fine (loanId, amount, isPaid) VALUES (@loanId, @amount, @isPaid) SELECT CAST(SCOPE_IDENTITY() AS INT)";

            using (var connection = _dbo.CreateConnection())
            {
                connection.Open();
                
                using(var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var loanId = await connection.ExecuteScalarAsync<int>(query1, loan, transaction);

                        fine.loanId = loanId;
                        await connection.ExecuteAsync(query2, fine, transaction);

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<IEnumerable<GetLoanDetails>> GetLoanDetails()
        {
            var query = "SELECT l.loanId, l.loanDate, l.returnDate, l.isReturned, " +
                "b.bookId, b.title, b.author, b.publishedYear, " +
                "u.userId, u.name, u.age, u.address, u.email FROM Loan l " +
                "INNER JOIN Book b ON b.bookId = l.bookId " +
                "INNER JOIN Users u ON u.userId = l.userId";

            using(var connection = _dbo.CreateConnection())
            {
                return await connection.QueryAsync<GetLoanDetails, GetBookWithoutGenreDto, UsersDataDto, GetLoanDetails>(query,
                    (loan, book, user) =>
                    {
                        loan.book = book;
                        loan.user = user;
                        return loan;
                    },
                   splitOn: "bookId,userId"
                    );
            }
        }

        public async Task<GetLoanDetails> GetLoanDetailsById(int loanId)
        {
            var query = "SELECT l.loanId, l.loanDate, l.returnDate, l.isReturned, " +
                "b.bookId, b.title, b.author, b.publishedYear, " +
                "u.userId, u.name, u.age, u.address, u.email FROM Loan l " +
                "INNER JOIN Book b ON b.bookId = l.bookId " +
                "INNER JOIN Users u ON u.userId = l.userId " +
                "WHERE l.loanId = @loanId";

            using(var connection = _dbo.CreateConnection())
            {
                var result = await connection.QueryAsync<GetLoanDetails, GetBookWithoutGenreDto, UsersDataDto, GetLoanDetails>(query,
                    (loan, book, user) =>
                    {
                        loan.book = book;
                        loan.user = user;
                        return loan;
                    },
                    new { loanId },
                    splitOn: "bookId,userId"
                    );

                return result.FirstOrDefault();
            }
        }

       public async Task<IEnumerable<GetUserLoanDetailsDto>> GetLoanDetailsByUserId(int userId)
        {
            var query = "SELECT b.bookId, b.title, b.author, b.publishedYear, l.loanId, l.loanDate, l.returnDate, l.isReturned, f.isPaid FROM Loan l " +
                "INNER JOIN Book b ON b.bookId = l.bookId " +
                "INNER JOIN Fine f ON f.loanId = l.loanId " +
                "WHERE l.userId = @userId";

            using(var connection = _dbo.CreateConnection())
            {
                return await connection.QueryAsync<GetUserLoanDetailsDto>(query, new { userId });
            }
        }

        public async Task<IEnumerable<GetMyLoanDetailsDto>> GetMyLoanDetails(ClaimsPrincipal User)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var query = "SELECT b.title, b.author, b.publishedYear, l.loanDate, l.returnDate, l.isReturned, f.amount, f.isPaid FROM Loan l " +
                "INNER JOIN Book b ON b.bookId = l.bookId " +
                "INNER JOIN Fine f ON f.loanId = l.loanId " +
                "WHERE l.userId = @userId";
            
            using(var connection = _dbo.CreateConnection())
            {
                return await connection.QueryAsync<GetMyLoanDetailsDto>(query, new { userId });
            }
        }

        public async Task UpdateLoanDetails(Loan loan, Fine fine, int loanId)
        {
            var query1 = "UPDATE Loan SET isReturned = @isReturned WHERE loanId = @loanId";

            var query2 = "UPDATE Fine SET isPaid = @isPaid WHERE loanId = @loanId";

            using(var connection = _dbo.CreateConnection())
            {
                connection.Open();
                using(var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.ExecuteAsync(query1, new { loanId, loan.isReturned }, transaction);
                        await connection.ExecuteAsync(query2, new { loanId, fine.isPaid }, transaction);

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
