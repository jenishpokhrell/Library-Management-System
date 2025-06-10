using DapperLearn.DTOs.Fine;
using DapperLearn.DTOs.Response;
using DapperLearn.Interfaces.IRepositories;
using DapperLearn.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.Services
{
    public class FineService : IFineService
    {
        private readonly IFineRepository _fineRepository;
        private readonly ILoanRepository _loanRepository;

        public FineService(IFineRepository fineRepository, ILoanRepository loanRepository)
        {
            _fineRepository = fineRepository;
            _loanRepository = loanRepository;
        }

        public Task<GeneralResponseDto> AddFineDetailsAsync(FineDto addFineDto, int loanId)
        {
            throw new NotImplementedException();
        }
        /*public async Task<GeneralResponseDto> AddFineDetailsAsync(FineDto addFineDto, int loanId)
{
   var loan = await _loanRepository.get
}*/
    }
}
