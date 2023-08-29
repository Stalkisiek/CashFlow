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
[Route("[controller]s")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthRepository _authRepository;

    public UserController(IUserService userService, IAuthRepository authRepository)
    {
        _userService = userService;
        _authRepository = authRepository;
    }
    
    // [AllowAnonymous]
    // [HttpPost]
    // public async Task<ActionResult<ServiceResponse<int>>> Register(RegisterUserDto registerUserDto)
    // {
    //     var response = await _authRepository.Register(registerUserDto);
    //     return StatusCode(response.StatusCode, response);
    // }
    //
    // [AllowAnonymous]
    // [HttpPost]
    // [Route("Login")]
    // public async Task<ActionResult<ServiceResponse<string>>> Login(LoginUserDto loginUserDto)
    // {
    //     var response = await _authRepository.Login(loginUserDto);
    //     return StatusCode(response.StatusCode, response);
    // }
    
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
    
    [HttpPut("authLvl")]
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

    [HttpDelete]
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