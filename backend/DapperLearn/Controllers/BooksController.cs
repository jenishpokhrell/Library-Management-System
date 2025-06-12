using DapperLearn.DTOs.Books;
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
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        [Route("AddBook")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddBookWithGenres([FromBody] AddBookDto addBookDto)
        {
            var result = await _bookService.AddBookAsync(addBookDto);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.Message);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllBookDetails")]
        [Authorize]
        public async Task<IActionResult> GetAllBooksDetails()
        {
            var result = await _bookService.GetAllBooksDetailsAsync();
            if(result is null)
            {
                return NotFound("Books not found");
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("GetBookById/{bookId}")]
        [Authorize]
        public async Task<IActionResult> GetBookById(int bookId)
        {
            var result = await _bookService.GetBookDetailsByIdAsync(bookId);
            if (result is null)
            {
                return NotFound("Book not found");
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetBookByTitle/{title}")]
        [Authorize]
        public async Task<IActionResult> GetBookByTitle(string title)
        {
            var result = await _bookService.GetBookDetailsByTitleAsync(title);
            if (result is null)
            {
                return NotFound("Book not found");
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("GetBookByGenre/{genreName}")]
        [Authorize]
        public async Task<IActionResult> GetBookByGenre(string genreName)
        {
            var result = await _bookService.GetBookByGenre(genreName);
            if (result is null)
            {
                return NotFound("Book not found");
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateBookDetails/{bookId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBook(AddBookDto updateBookDto, int bookId)
        {
            var result = await _bookService.UpdateBookAsync(updateBookDto, bookId);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Message);
            }

            return Ok(result);
        }

        [HttpDelete]
        [Route("DeleteBook/{bookId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            var result = await _bookService.DeleteBookAsync(bookId);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Message);
            }

            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
