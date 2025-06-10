using DapperLearn.DTOs.Fine;
using DapperLearn.DTOs.Loan;
using DapperLearn.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DapperLearn.Interfaces.IServices
{
    public interface ILoanService
    {
        Task<GeneralResponseDto> AddBookLoanDetailsAsync(int bookId, int userId);

        Task<IEnumerable<GetLoanDetails>> GetFullLoanDetails();

        Task<IEnumerable<GetUserLoanDetailsDto>> GetLoanDetailsByUserIdAsync(int userId);

        Task<GetLoanDetails> GetFullLoanDetailsById(int loanId);

        Task<IEnumerable<GetMyLoanDetailsDto>> GetMyLoanDetails(ClaimsPrincipal User);

        Task<GeneralResponseDto> UpdateLoanDetailsAsync(UpdateLoanDetailsDto updateLoanDetailsDto, int loanId);
    }
}
