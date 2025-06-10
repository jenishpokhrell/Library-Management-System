using DapperLearn.DTOs.Books;
using DapperLearn.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.Interfaces.IServices
{
    public interface IBookService
    {
        Task<GeneralResponseDto> AddBookAsync(AddBookDto addBookDto);

        Task<IEnumerable<GetBookDto>> GetAllBooksDetailsAsync();

        Task<GetBookDto> GetBookDetailsByIdAsync(int bookId);

        Task<IEnumerable<GetBookDto>> GetBookByGenre(string genreName);

        Task<GeneralResponseDto> UpdateBookAsync(AddBookDto updateBookDto, int bookId);

        Task<GeneralResponseDto> DeleteBookAsync(int bookId);
    }
}
