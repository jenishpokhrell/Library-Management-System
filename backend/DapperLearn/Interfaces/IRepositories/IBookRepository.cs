using DapperLearn.DTOs.Books;
using DapperLearn.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.Interfaces.IRepositories
{
    public interface IBookRepository
    {
        Task<int> AddBook(Book book);

        Task<int> GetorAddGenre(string name);

        Task AddBookGenre(int bookId, int genreId);

        Task<Book> GetBookById(int bookId);

        Task<IEnumerable<Book>> GetBookDetails();

        Task<IEnumerable<Book>> GetBookByGenre(string genreName);

        Task UpdateBook(AddBookDto updateBookDto, int bookId);

        Task DeleteBook(int bookId);


    }
}
