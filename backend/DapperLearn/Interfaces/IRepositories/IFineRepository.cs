using DapperLearn.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.Interfaces.IRepositories
{
    public interface IFineRepository
    {
        Task AddFine(Fine fine);
    }
}
