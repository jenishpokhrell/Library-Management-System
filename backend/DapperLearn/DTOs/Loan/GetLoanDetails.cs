using DapperLearn.DTOs.Auth;
using DapperLearn.DTOs.Books;
using DapperLearn.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.DTOs.Loan
{
    public class GetLoanDetails
    {
        public int loanId { get; set; }
        public GetBookWithoutGenreDto book { get; set; }
        public UsersDataDto user { get; set; }
        public DateTime loanDate { get; set; }
        public DateTime returnDate { get; set; }
        public bool isReturned { get; set; }
    }
}
