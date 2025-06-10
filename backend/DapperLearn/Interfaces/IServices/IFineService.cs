using DapperLearn.DTOs.Fine;
using DapperLearn.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.Interfaces.IServices
{
    public interface IFineService
    {
        Task<GeneralResponseDto> AddFineDetailsAsync(FineDto addFineDto, int loanId);
    }
}
