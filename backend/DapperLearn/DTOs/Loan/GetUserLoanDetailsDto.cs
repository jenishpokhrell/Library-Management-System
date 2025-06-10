using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.DTOs.Loan
{
    public class GetUserLoanDetailsDto
    {
        public int bookId { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string publishedYear { get; set; }
        public int loanId { get; set; }
        public DateTime loanDate { get; set; }
        public DateTime returnDate { get; set; }
        public bool isReturned { get; set; }
    }
}
