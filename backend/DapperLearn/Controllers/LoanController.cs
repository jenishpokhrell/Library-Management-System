using DapperLearn.DTOs.Loan;
using DapperLearn.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DapperLearn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly ILoanService _loanServices;

        public LoanController(ILoanService loanServices)
        {
            _loanServices = loanServices;
        }

        [HttpPost]
        [Route("AddBookLoanDetails/{bookId}/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddBookLoanDetails(int bookId, int userId)
        {
            var result = await _loanServices.AddBookLoanDetailsAsync(bookId, userId);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Message);
            }

            return StatusCode(result.StatusCode, result.Message);
        }

        [HttpGet]
        [Route("GetLoanDetails")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetLoanDetails()
        {
            var result = await _loanServices.GetFullLoanDetails();

            if(result is null)
            {
                return NotFound("No Loan Request Found");
            }

            return Ok(result);
        }


        [HttpGet]
        [Route("GetMyLoanDetails")]
        [Authorize]
        public async Task<IActionResult> GetMyLoanDetails()
        {
            var result = await _loanServices.GetMyLoanDetails(User);

            if (result is null)
            {
                return NotFound("No Loan Request Found");
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("GetLoanDetailsById/{loanId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetLoanDetailsById(int loanId)
        {
            var result = await _loanServices.GetFullLoanDetailsById(loanId);

            if (result is null)
            {
                return NotFound("No Loan Request Found");
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("GetLoanDetailsByuserId/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetLoanDetailsByUserId(int userId)
        {
            var result = await _loanServices.GetLoanDetailsByUserIdAsync(userId);

            if (result is null)
            {
                return NotFound("No Loan Request Found");
            }

            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateLoanDetails/{loanId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateLoanDetails([FromBody] UpdateLoanDetailsDto updateLoanDetailsDto, int loanId)
        {
            var result = await _loanServices.UpdateLoanDetailsAsync(updateLoanDetailsDto, loanId);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Message);
            }

            return StatusCode(result.StatusCode, result.Message);
        }

    }
}
