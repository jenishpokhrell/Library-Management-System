using DapperLearn.DTOs.Auth;
using DapperLearn.DTOs.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.Entities
{
    public class Loan
    {
        public int loanId { get; set; }
        public int bookId { get; set; }
        public GetBookWithoutGenreDto book { get; set; }
        public int userId { get; set; }
        public UsersDataDto user { get; set; }
        public DateTime loanDate { get; set; }
        public DateTime returnDate { get; set; }
        public bool isReturned { get; set; }
    }
}
