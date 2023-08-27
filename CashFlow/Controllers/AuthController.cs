using CashFlow.Dtos.Account;
using CashFlow.Dtos.Authorization;
using CashFlow.Models;
using CashFlow.Services.AuthServices;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<ServiceResponse<int>>> Register(RegisterUserDto registerUserDto)
    {
        return Ok(await _authRepository.Register(registerUserDto));
    }
    
    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<ServiceResponse<string>>> Login(LoginUserDto loginUserDto)
    {
        return Ok(await _authRepository.Login(loginUserDto));
    }
}