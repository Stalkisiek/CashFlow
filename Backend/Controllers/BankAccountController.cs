using CashFlow.Dtos.BankAccount;
using CashFlow.Models;
using CashFlow.Services.BankAccountServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]s")]
public class BankAccountController : ControllerBase
{
    private readonly IBankAccountService _bankAccountService;

    public BankAccountController(IBankAccountService bankAccountService)
    {
        _bankAccountService = bankAccountService;
    }
    
    [HttpPost]
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

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<GetBankAccountDto>>>> GetAll(int? id, int? type)
    {
        var response = await _bankAccountService.GetAll(id, type);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [Route("self")]
    public async Task<ActionResult<ServiceResponse<List<GetBankAccountDto>>>> GetAllWithinUser()
    {
        var response = await _bankAccountService.GetAllWithinUser();
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut]
    public async Task<ActionResult<ServiceResponse<GetBankAccountDto>>> UpdateBankAccount(
        int id, int type, double balance, double credit)
    {
        UpdateBankAccountDto updateBankAccountDto = new()
        {
            Id = id,
            Type = (BankAccountType) type,
            Balance = balance,
            CreditBalance = credit
        };
        
        var response = await _bankAccountService.UpdateBankAccount(updateBankAccountDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut]
    [Route("{id:int}/balance/add")]
    public async Task<ActionResult<ServiceResponse<GetBankAccountDto>>> AddBalance(int id, double amount)
    {
        var response = await _bankAccountService.AddBalance(id, amount);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut]
    [Route("{id:int}/balance/subtract")]
    public async Task<ActionResult<ServiceResponse<GetBankAccountDto>>> SubtractBalance(int id, double amount)
    {
        var response = await _bankAccountService.SubtractBalance(id, amount);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPut]
    [Route("{id:int}/transfer")]
    public async Task<ActionResult<ServiceResponse<GetBankAccountDto>>> SubtractBalance(int id, int targetId, double amount)
    {
        var response = await _bankAccountService.TransferBalance(id, targetId, amount);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPut]
    [Route("{id:int}/credit/add")]
    public async Task<ActionResult<ServiceResponse<GetBankAccountDto>>> AddCredit(int id, double amount)
    {
        var response = await _bankAccountService.AddCredit(id, amount);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPut]
    [Route("{id:int}/credit/subtract")]
    public async Task<ActionResult<ServiceResponse<GetBankAccountDto>>> PayCredit(int id, double amount)
    {
        var response = await _bankAccountService.PayCredit(id, amount);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult<ServiceResponse<List<GetBankAccountDto>>>> Delete(int id)
    {
        var response = await _bankAccountService.DeleteBankAccount(id);
        return StatusCode(response.StatusCode, response);
    }
}