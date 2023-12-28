using CashFlow.Dtos.Authorization;
using CashFlow.Models;

namespace CashFlow.Services.AuthServices;

public interface IAuthRepository
{
    Task<bool> UserExists(string email);
    Task<ServiceResponse<string>> Login(LoginUserDto loginUserDto);
    Task<ServiceResponse<string>> Register(RegisterUserDto registerUserDto);
    bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
}