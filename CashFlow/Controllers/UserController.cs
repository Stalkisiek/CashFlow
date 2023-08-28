using CashFlow.Dtos.Account;
using CashFlow.Dtos.User;
using CashFlow.Models;
using CashFlow.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Controllers;

[Authorize] // Moved Authorized attribute to controller level
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]   
    public async Task<ActionResult<ServiceResponse<List<GetUserDto>>>> GetAllUsers()
    {
        var response = await _userService.GetAllUsers();
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpGet("current")]
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
    
    [HttpPut("updateEmail")]
    public async Task<ActionResult<ServiceResponse<GetUserDto>>> UpdateEmail(UpdateUserEmailDto updateUserEmailDto)
    {
        var response = await _userService.UpdateUserEmail(updateUserEmailDto);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPut("updateNames")]
    public async Task<ActionResult<ServiceResponse<GetUserDto>>> UpdateNames(UpdateUserNamesDto updateUserNamesDto)
    {
        var response = await _userService.UpdateUserNames(updateUserNamesDto);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPut("updateAuthLvl")]
    public async Task<ActionResult<ServiceResponse<GetUserDto>>> UpdateAuthLvl(UpdateUserAuthorizationLevelDto updateUserAuthLvlDto)
    {
        var response = await _userService.UpdateUserAuthorizationLevel(updateUserAuthLvlDto);
        return StatusCode(response.StatusCode, response);
    }
}