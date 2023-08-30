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
    public async Task<ActionResult<ServiceResponse<List<GetBankAccountDto>>>> GetAll()
    {
        var response = await _bankAccountService.GetAll();
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [Route("currentUser")]
    public async Task<ActionResult<ServiceResponse<List<GetBankAccountDto>>>> GetAllWithinUser()
    {
        var response = await _bankAccountService.GetAllWithinUser();
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut]
    public async Task<ActionResult<ServiceResponse<GetBankAccountDto>>> UpdateBankAccount(
        UpdateBankAccountDto updateBankAccountDto)
    {
        var response = await _bankAccountService.UpdateBankAccount(updateBankAccountDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut]
    [Route("{id:int}/addBalance")]
    public async Task<ActionResult<ServiceResponse<GetBankAccountDto>>> AddBalance(int id, double amount)
    {
        var response = await _bankAccountService.AddBalance(id, amount);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut]
    [Route("{id:int}/subtractBalance")]
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
    [Route("{id:int}/credit")]
    public async Task<ActionResult<ServiceResponse<GetBankAccountDto>>> AddCredit(int id, double amount)
    {
        var response = await _bankAccountService.AddCredit(id, amount);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPut]
    [Route("{id:int}/paycredit")]
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