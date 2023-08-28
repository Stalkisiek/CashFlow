using CashFlow.Dtos.BankAccount;
using CashFlow.Models;

namespace CashFlow.Services.BankAccountServices;

public interface IBankAccountService
{
    Task<ServiceResponse<List<GetBankAccountDto>>> GetAllBankAccounts();
    Task<ServiceResponse<List<GetBankAccountDto>>> GetAllWithinUser();
    Task<ServiceResponse<GetBankAccountDto>> GetBankAccountById(int id);
    //Task<ServiceResponse<GetBankAccountDto>> CreateBankAccount(CreateBankAccountDto createBankAccountDto);
    
}