using CashFlow.Dtos.Authorization;

namespace CashFlow.Services.AuthServices;

public class AuthRepository : IAuthRepository
{
    public Task<bool> UserExists(string email)
    {
        throw new NotImplementedException();
    }

    public Task<string> Login(LoginUserDto loginUserDto)
    {
        throw new NotImplementedException();
    }

    public Task<int> Register(RegisterUserDto registerUserDto)
    {
        throw new NotImplementedException();
    }
}