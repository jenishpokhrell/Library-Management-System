using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.Entities
{
    public class Fine
    {
        public int fineId { get; set; }
        public int loanId { get; set; }
        public int amount { get; set; }
        public bool isPaid { get; set; }
    }
}
