using CashFlow.Dtos.BankAccount;
using CashFlow.Models;

namespace CashFlow.Services.BankAccountServices;

public interface IBankAccountService
{
    Task<ServiceResponse<List<GetBankAccountDto>>> GetAll();
    Task<ServiceResponse<List<GetBankAccountDto>>> GetAllWithinUser();
    Task<ServiceResponse<GetBankAccountDto>> GetBankAccountById(int id);
    Task<ServiceResponse<GetBankAccountDto>> CreateBankAccount(AddBankAccountDto addBankAccountDto);
    Task<ServiceResponse<GetBankAccountDto>> UpdateBankAccount(UpdateBankAccountDto updateBankAccountDto);
    Task<ServiceResponse<List<GetBankAccountDto>>> DeleteBankAccount(int id);
    Task<ServiceResponse<GetBankAccountDto>> AddBalance(int id, decimal amount);
    Task<ServiceResponse<GetBankAccountDto>> SubtractBalance(int id, decimal amount);
    Task<ServiceResponse<GetBankAccountDto>> TransferBalance(int id, int targetId, decimal amount);
    Task<ServiceResponse<GetBankAccountDto>> AddCredit(int id, decimal amount);
    Task<ServiceResponse<GetBankAccountDto>> SubtractCredit(int id, decimal amount); // Move money from balance to credit (deleting credit)
}