using Dapper;
using DapperLearn.Context;
using DapperLearn.Entities;
using DapperLearn.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.Repositories
{
    public class FineRepository : IFineRepository
    {
        private readonly DapperContext _dbo;

        public FineRepository(DapperContext dbo)
        {
            _dbo = dbo;
        }
        public async Task AddFine(Fine fine)
        {
            var query = "INSERT INTO Fine (loanId, amount, isPaid) VALUES (@loanId, @amount, @isPaid)";

            using(var connection = _dbo.CreateConnection())
            {
                await connection.ExecuteAsync(query, fine);
            }
        }
    }
}
