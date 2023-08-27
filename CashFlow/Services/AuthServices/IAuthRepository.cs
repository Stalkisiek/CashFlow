using CashFlow.Dtos.Authorization;

namespace CashFlow.Services.AuthServices;

public interface IAuthRepository
{
    Task<bool> UserExists(string email);
    Task<string> Login(LoginUserDto loginUserDto);
    Task<int> Register(RegisterUserDto registerUserDto);
}