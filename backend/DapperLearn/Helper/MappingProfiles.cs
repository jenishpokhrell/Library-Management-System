using AutoMapper;
using DapperLearn.DTOs;
using DapperLearn.DTOs.Auth;
using DapperLearn.DTOs.Books;
using DapperLearn.DTOs.Loan;
using DapperLearn.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.Helper
{
    public class MappingProfiles : Profile
    {
       public MappingProfiles()
        {
            CreateMap<Users, UsersDto>().ReverseMap();
            CreateMap<Users, UsersDataDto>().ReverseMap();
            CreateMap<Book, GetBookDto>().ReverseMap();
            CreateMap<Book, GetBookWithoutGenreDto>().ReverseMap();
            CreateMap<Loan, GetLoanDetails>()
                .ForMember(ld => ld.loanDate, loandate => loandate.MapFrom(src => src.loanDate.Date))
                .ForMember(rd => rd.returnDate, returndate => returndate.MapFrom(src => src.returnDate.Date))
                .ReverseMap();
        }
    }
}
