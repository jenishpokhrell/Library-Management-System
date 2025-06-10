using DapperLearn.DTOs.Fine;
using DapperLearn.DTOs.Loan;
using DapperLearn.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DapperLearn.Interfaces.IRepositories
{
    public interface ILoanRepository
    {
        Task AddLoanDetails(Loan loan, Fine fine);

        Task<IEnumerable<GetLoanDetails>> GetLoanDetails();

        Task<IEnumerable<GetMyLoanDetailsDto>> GetMyLoanDetails(ClaimsPrincipal User);

        Task<GetLoanDetails> GetLoanDetailsById(int loanId);

        Task<IEnumerable<GetUserLoanDetailsDto>> GetLoanDetailsByUserId(int userId);

        Task UpdateLoanDetails(Loan loan, Fine fine, int loanId);
    }
}
