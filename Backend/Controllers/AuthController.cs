using CashFlow.Dtos.Account;
using CashFlow.Dtos.Authorization;
using CashFlow.Models;
using CashFlow.Services.AuthServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace CashFlow.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepository;

    public AuthController(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }
    
    
    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult<ServiceResponse<int>>> Register([FromBody] RegisterUserDto registerUserDto)
    {
        var response = await _authRepository.Register(registerUserDto);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<ServiceResponse<string>>> Login(LoginUserDto loginUserDto)
    {
        var response = await _authRepository.Login(loginUserDto);
        return StatusCode(response.StatusCode, response);
    }
}