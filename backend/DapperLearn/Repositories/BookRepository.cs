using Dapper;
using DapperLearn.Context;
using DapperLearn.DTOs.Books;
using DapperLearn.Entities;
using DapperLearn.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly DapperContext _dbo;

        public BookRepository(DapperContext dbo)
        {
            _dbo = dbo;
        }

        public async Task<int> AddBook(Book book)
        {
            var query = "INSERT INTO Book (title, author, publishedYear, isAvailable) VALUES (@title, @author, " +
                "@publishedYear, @isAvailable) SELECT CAST(SCOPE_IDENTITY() AS INT)";

            using (var connection = _dbo.CreateConnection())
            {
                return await connection.ExecuteScalarAsync<int>(query, book);
            }
        }

        public async Task<int> GetorAddGenre(string name)
        {
            var checkQuery = "SELECT genreId from Genre WHERE name = @name";

            var query = "INSERT INTO Genre (name) VALUES (@name) SELECT CAST(SCOPE_IDENTITY() AS INT)";

            using(var connection = _dbo.CreateConnection())
            {
                var genreId = await connection.QueryFirstOrDefaultAsync<int?>(checkQuery, new { name });
                if (genreId.HasValue)
                {
                    return genreId.Value;
                }
                return await connection.ExecuteScalarAsync<int>(query, new { name });
            }
        }

        public async Task AddBookGenre(int bookId, int genreId)
        {
            var query = "INSERT INTO BookGenres (bookId, genreId) VALUES (@bookId, @genreId)";

            using(var connection = _dbo.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { bookId, genreId });
            }
        }

        public async Task<IEnumerable<Book>> GetBookDetails()
        {
            var query = "SELECT b.bookId, b.title, b.author, b.publishedYear, b.isAvailable, STRING_AGG(g.name, ', ') AS genres FROM Book b " +
                "INNER JOIN BookGenres bg ON b.bookId = bg.bookId " +
                "INNER JOIN Genre g ON g.genreId = bg.genreId " +
                "GROUP BY " +
                "b.bookId, b.title, b.author, b.publishedYear, b.isAvailable";

            using(var connection = _dbo.CreateConnection())
            {
               return await connection.QueryAsync<Book>(query);
            }
        }

        public async Task UpdateBook(AddBookDto updateBookDto, int bookId)
        {
            var query = "UPDATE Book SET title = @title, author = @author, publishedYear = @publishedYear, isAvailable = @isAvailable WHERE bookId = @bookId";

            var parameters = new DynamicParameters();
            parameters.Add("bookId", bookId, DbType.Int32);
            parameters.Add("title", updateBookDto.title, DbType.String);
            parameters.Add("author", updateBookDto.author, DbType.String);
            parameters.Add("publishedYear", updateBookDto.publishedYear, DbType.Int32);
            parameters.Add("isAvailable", updateBookDto.isAvailable, DbType.Boolean);

            using (var connection = _dbo.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<Book> GetBookById(int bookId)
        {
            var query = "SELECT b.bookId, b.title, b.author, b.publishedYear, b.isAvailable, STRING_AGG(g.name, ', ') AS genres " +
                "FROM Book b " +
                "INNER JOIN BookGenres AS bg ON b.bookId = bg.bookId " +
                "INNER JOIN Genre AS g on g.genreId = bg.genreId WHERE b.bookId = @bookId " +
                "GROUP BY b.bookId, b.title, b.author, b.publishedYear, b.isAvailable";

            using(var connection = _dbo.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Book>(query, new { bookId });
            }
        }

        public async Task<IEnumerable<Book>> GetBookByGenre(string genreName)
        {
            var query = "SELECT b.bookId, b.title, b.author, b.publishedYear, b.isAvailable, STRING_AGG(g.name, ', ') AS genres FROM Book b " +
                "INNER JOIN BookGenres AS bg ON b.bookId = bg.bookId " +
                "INNER JOIN Genre AS g ON g.genreId = bg.genreId WHERE g.name = @genreName " +
                "GROUP BY b.bookId, b.title, b.author, b.publishedYear, b.isAvailable";

            using (var connection = _dbo.CreateConnection())
            {
                return await connection.QueryAsync<Book>(query, new { genreName });
            }
        }

        public async Task DeleteBook(int bookId)
        {
            var query1 = "DELETE FROM BookGenres WHERE bookId = @bookId";

            var query2 = "DELETE FROM Book WHERE bookId = @bookId";

            using(var connection = _dbo.CreateConnection())
            {
                connection.Open();
                using(var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.ExecuteAsync(query1, new { bookId }, transaction);
                        await connection.ExecuteAsync(query2, new { bookId }, transaction);

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

        public async Task<Book> GetBookByTitle(string title)
        {
            var query = "SELECT b.bookId, b.title, b.author, b.publishedYear, b.isAvailable, STRING_AGG(g.name, ', ') AS genres " +
                "FROM Book b " +
                "INNER JOIN BookGenres AS bg ON b.bookId = bg.bookId " +
                "INNER JOIN Genre AS g on g.genreId = bg.genreId WHERE b.title LIKE @title " +
                "GROUP BY b.bookId, b.title, b.author, b.publishedYear, b.isAvailable";

            using (var connection = _dbo.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Book>(query, new { title = $"{title}%" });
            }
        }
    }
}
