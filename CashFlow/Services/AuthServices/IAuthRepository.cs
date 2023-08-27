using CashFlow.Dtos.Authorization;
using CashFlow.Models;

namespace CashFlow.Services.AuthServices;

public interface IAuthRepository
{
    Task<bool> UserExists(string email);
    Task<ServiceResponse<string>> Login(LoginUserDto loginUserDto);
    Task<ServiceResponse<int>> Register(RegisterUserDto registerUserDto);
}