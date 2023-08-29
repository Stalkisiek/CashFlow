using AutoMapper;
using CashFlow.Dtos.Account;
using CashFlow.Dtos.Authorization;
using CashFlow.Dtos.Request;
using CashFlow.Models;

namespace CashFlow;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // CreateMap<Source, Destination>();
        CreateMap<RegisterUserDto, User>();
        CreateMap<User, GetUserDto>();
        CreateMap<Request, GetRequestDto>();
        CreateMap<AddRequestDto, Request>();
        CreateMap<Request, GetPreviousRequestDto>();
        CreateMap<PreviousRequest, GetPreviousRequestDto>();
    }
}