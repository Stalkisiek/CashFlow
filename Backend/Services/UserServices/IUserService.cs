using CashFlow.Dtos.Account;
using CashFlow.Dtos.Authorization;
using CashFlow.Dtos.User;
using CashFlow.Models;

namespace CashFlow.Services.UserServices;

public interface IUserService
{
    Task<ServiceResponse<List<GetUserDto>>> GetAllUsers(int? id, int? authLvl, string? name, string? surname, string? email);
    Task<ServiceResponse<GetUserDto>> GetCurrentUser();
    Task<ServiceResponse<GetUserDto>> GetUserById(int id);
    Task<ServiceResponse<GetUserDto>> UpdateUserEmail(UpdateUserEmailDto updatedUserEmail);
    Task<ServiceResponse<GetUserDto>> UpdateUserNames(UpdateUserNamesDto updateUserNamesDto);
    Task<ServiceResponse<GetUserDto>> UpdateUserAuthorizationLevel(UpdateUserAuthorizationLevelDto updateUserAuthorizationLevelDto);
    Task<ServiceResponse<GetUserDto>> UpdateUserPassword(UpdateUserPasswordDto updateUserPasswordDto);
    Task<ServiceResponse<string>> DeleteCurrentUser();
    Task<ServiceResponse<List<GetUserDto>>> DeleteUserById(int id);
}