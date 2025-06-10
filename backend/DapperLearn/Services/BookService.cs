using AutoMapper;
using DapperLearn.DTOs.Books;
using DapperLearn.DTOs.Response;
using DapperLearn.Entities;
using DapperLearn.Interfaces.IRepositories;
using DapperLearn.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task<GeneralResponseDto> AddBookAsync(AddBookDto addBookDto)
        {
            try
            {
                var newBook = new Book
                {
                    title = addBookDto.title,
                    author = addBookDto.author,
                    publishedYear = addBookDto.publishedYear,
                    isAvailable = addBookDto.isAvailable
                };

                var bookId = await _bookRepository.AddBook(newBook);

                foreach(var genreName in addBookDto.genres)
                {
                    int genreId = await _bookRepository.GetorAddGenre(genreName);
                    await _bookRepository.AddBookGenre(bookId, genreId);
                }

                return new GeneralResponseDto
                {
                    IsSuccess = true,
                    StatusCode = 201,
                    Message = "Book and genres inserted successfully."
                };
            }
            catch (Exception)
            {
                return new GeneralResponseDto
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = "Failed to add book"
                };
            }
        }

        public async Task<IEnumerable<GetBookDto>> GetAllBooksDetailsAsync()
        {
            var books = await _bookRepository.GetBookDetails();

            return _mapper.Map<IEnumerable<GetBookDto>>(books);

        }

        public async Task<IEnumerable<GetBookDto>> GetBookByGenre(string genreName)
        {
            var book = await _bookRepository.GetBookByGenre(genreName);

            return _mapper.Map<IEnumerable<GetBookDto>>(book);
        }

        public async Task<GetBookDto> GetBookDetailsByIdAsync(int bookId)
        {
            var book = await _bookRepository.GetBookById(bookId);

            return _mapper.Map<GetBookDto>(book);
        }

        public async Task<GeneralResponseDto> UpdateBookAsync(AddBookDto updateBookDto, int bookId)
        {
            var book = await _bookRepository.GetBookById(bookId);

            if(book is null)
            {
                return new GeneralResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "Book not found."
                };
            }

            foreach (var genreName in updateBookDto.genres)
            {
                int genreId = await _bookRepository.GetorAddGenre(genreName);
                await _bookRepository.AddBookGenre(bookId, genreId);
            }

            await _bookRepository.UpdateBook(updateBookDto, bookId);

            return new GeneralResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Book updated successfully."
            };

        }

        public async Task<GeneralResponseDto> DeleteBookAsync(int bookId)
        {
            var book = await _bookRepository.GetBookById(bookId);

            if(book is null)
            {
                return new GeneralResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Book doesn't exist."
                };
            }

            await _bookRepository.DeleteBook(bookId);

            return new GeneralResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Book deleted successfully."
            };
        }
    }
}
