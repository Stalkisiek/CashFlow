using CashFlow.Dtos.BankAccount;
using CashFlow.Models;
using CashFlow.Services.BankAccountServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Controllers;

[Authorize]
[ApiController]
[Route("[controller]s")]
public class BankAccountController : ControllerBase
{
    private readonly IBankAccountService _bankAccountService;

    public BankAccountController(IBankAccountService bankAccountService)
    {
        _bankAccountService = bankAccountService;
    }
    
    [HttpPut]
    public async Task<ActionResult<ServiceResponse<GetBankAccountDto>>> CreateBankAccount(
        AddBankAccountDto addBankAccountDto)
    {
        var response = await _bankAccountService.CreateBankAccount(addBankAccountDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<ServiceResponse<GetBankAccountDto>>> GetById(int id)
    {
        var response = await _bankAccountService.GetBankAccountById(id);
        return StatusCode(response.StatusCode, response);
    }
}