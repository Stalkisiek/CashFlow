using CashFlow.Dtos.Account;
using CashFlow.Dtos.Authorization;
using CashFlow.Dtos.User;
using CashFlow.Models;
using CashFlow.Services.AuthServices;
using CashFlow.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Controllers;

[Authorize] // Moved Authorized attribute to controller level
[ApiController]
[Route("api/[controller]s")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthRepository _authRepository;

    public UserController(IUserService userService, IAuthRepository authRepository)
    {
        _userService = userService;
        _authRepository = authRepository;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<GetUserDto>>>> GetAllUsers(int? id = null, int? authLvl = null, string name = null, string surname = null, string email = null)
    {
        var response = await _userService.GetAllUsers(id, authLvl, name, surname, email);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("self")]
    public async Task<ActionResult<ServiceResponse<GetUserDto>>> GetCurrentUser()
    {
        var response = await _userService.GetCurrentUser();
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServiceResponse<GetUserDto>>> GetUserById(int id)
    {
        var response = await _userService.GetUserById(id);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPut("email")]
    public async Task<ActionResult<ServiceResponse<GetUserDto>>> UpdateEmail(UpdateUserEmailDto updateUserEmailDto)
    {
        var response = await _userService.UpdateUserEmail(updateUserEmailDto);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPut("names")]
    public async Task<ActionResult<ServiceResponse<GetUserDto>>> UpdateNames(UpdateUserNamesDto updateUserNamesDto)
    {
        var response = await _userService.UpdateUserNames(updateUserNamesDto);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPut("auth")]
    public async Task<ActionResult<ServiceResponse<GetUserDto>>> UpdateAuthLvl(UpdateUserAuthorizationLevelDto updateUserAuthLvlDto)
    {
        var response = await _userService.UpdateUserAuthorizationLevel(updateUserAuthLvlDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("password")]
    public async Task<ActionResult<ServiceResponse<GetUserDto>>> UpdatePassword(
        UpdateUserPasswordDto updateUserPasswordDto)
    {
        var response = await _userService.UpdateUserPassword(updateUserPasswordDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("self")]
    public async Task<ActionResult<ServiceResponse<string>>> DeleteCurrentUser()
    {
        var response = await _userService.DeleteCurrentUser();
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<List<GetUserDto>>> DeleteUserById(int id)
    {
        var response = await _userService.DeleteUserById(id);
        return StatusCode(response.StatusCode, response);
    }
}