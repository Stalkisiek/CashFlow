using AutoMapper;
using CashFlow.Dtos.Account;
using CashFlow.Dtos.Authorization;
using CashFlow.Models;

namespace CashFlow;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // CreateMap<Source, Destination>();
        CreateMap<RegisterUserDto, User>();
        CreateMap<User, GetUserDto>();
    }
}