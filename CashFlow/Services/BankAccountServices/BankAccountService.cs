using CashFlow.Dtos.BankAccount;
using CashFlow.Models;

namespace CashFlow.Services.BankAccountServices;

public class BankAccountService : IBankAccountService
{
    public Task<ServiceResponse<List<GetBankAccountDto>>> GetAllBankAccounts()
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<List<GetBankAccountDto>>> GetAllWithinUser()
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<GetBankAccountDto>> GetBankAccountById(int id)
    {
        throw new NotImplementedException();
    }
}