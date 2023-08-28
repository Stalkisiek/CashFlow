using CashFlow.Dtos.Account;
using CashFlow.Dtos.User;
using CashFlow.Models;

namespace CashFlow.Services.UserServices;

public interface IUserService
{
    Task<ServiceResponse<List<GetUserDto>>> GetAllUsers();
    Task<ServiceResponse<GetUserDto>> GetUserById(int id);
    Task<ServiceResponse<GetUserDto>> UpdateUserEmail(UpdateUserEmailDto updatedUserEmail);
    Task<ServiceResponse<GetUserDto>> UpdateUserNames(UpdateUserNamesDto updateUserNamesDto);
    Task<ServiceResponse<List<GetUserDto>>> DeleteUser(int id);
}