using AutoMapper;
using DapperLearn.DTOs.Fine;
using DapperLearn.DTOs.Loan;
using DapperLearn.DTOs.Response;
using DapperLearn.Entities;
using DapperLearn.Interfaces.IRepositories;
using DapperLearn.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DapperLearn.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public LoanService(ILoanRepository loanRepository, IBookRepository bookRepository, IUserRepository userRepository, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<GeneralResponseDto> AddBookLoanDetailsAsync(int bookId, int userId)
        {
            var book = await _bookRepository.GetBookById(bookId);

            if(book is null)
            {
                return new GeneralResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Book for this id doesn't exist"
                };
            }

            if(book.isAvailable is false)
            {
                return new GeneralResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "The book isn't available"
                };
            }

            var user = await _userRepository.GetUserById(userId);

            if (user is null)
            {
                return new GeneralResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "User for this id doesn't exist"
                };
            }

            var newLoanDetails = new Loan()
            {
                bookId = book.bookId,
                userId = user.userId,
                loanDate = DateTime.Now.Date,
                returnDate = DateTime.Now.Date.AddDays(15),
                isReturned = false
            };

            var fineDetails = new Fine()
            {
                //loanId = newLoanDetails.loanId,
                amount = 350,
                isPaid = false,
            };

            await _loanRepository.AddLoanDetails(newLoanDetails, fineDetails);

            return new GeneralResponseDto()
            {
                IsSuccess = true,
                StatusCode = 201,
                Message = "Book loan and fine details added successfully."
            };
        }

        public async Task<IEnumerable<GetLoanDetails>> GetFullLoanDetails()
        {
            var loanDetails = await _loanRepository.GetLoanDetails();

            return _mapper.Map<IEnumerable<GetLoanDetails>>(loanDetails);
        }

        public async Task<GetLoanDetails> GetFullLoanDetailsById(int loanId)
        {
            var loanDetails = await _loanRepository.GetLoanDetailsById(loanId);

            return _mapper.Map<GetLoanDetails>(loanDetails);
        }

        public async Task<IEnumerable<GetUserLoanDetailsDto>> GetLoanDetailsByUserIdAsync(int userId)
        {
            var loanDetails = await _loanRepository.GetLoanDetailsByUserId(userId);

            return _mapper.Map<IEnumerable<GetUserLoanDetailsDto>>(loanDetails);
        }

        public async Task<IEnumerable<GetMyLoanDetailsDto>> GetMyLoanDetails(ClaimsPrincipal User)
        {
            var myLoanDetails = await _loanRepository.GetMyLoanDetails(User);

            return _mapper.Map<IEnumerable<GetMyLoanDetailsDto>>(myLoanDetails);
        }

        public async Task<GeneralResponseDto> UpdateLoanDetailsAsync(UpdateLoanDetailsDto updateLoanDetailsDto, int loanId)
        {
            var loan = await _loanRepository.GetLoanDetailsById(loanId);

            if(loan is null)
            {
                return new GeneralResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = "The loan request doesn't exist"
                };
            }

            var updateLoanDetails = new Loan()
            {
                isReturned = updateLoanDetailsDto.isReturned
            };

            var updateFine = new Fine
            {
                isPaid = updateLoanDetailsDto.isReturned == true ? true : false
            };

            await _loanRepository.UpdateLoanDetails(updateLoanDetails, updateFine, loanId);

            return new GeneralResponseDto()
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "The loan and fine status updated successfully."
            };

        }
    }
}
