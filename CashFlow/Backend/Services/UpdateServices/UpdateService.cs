using AutoMapper;
using CashFlow.Data;
using CashFlow.Models;

namespace CashFlow.Services.UpdateServices;

public class UpdateService : IUpdateService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public UpdateService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    
    public async Task<ServiceResponse<string>> UpdateAll()
    {
        var response = new ServiceResponse<string>();
        try
        {
            foreach (var user in _context.Users.ToList())
            {
                List<BankAccount> bankAccounts = await _context.BankAccounts
                    .Where(b => b.UserId == user.Id)
                    .ToListAsync();

                var requests = await _context.Requests
                    .Where(r => r.UserId == user.Id)
                    .ToListAsync();

                foreach (BankAccount bankAccount in bankAccounts)
                {
                    bankAccount.Name = user.Name + "_" + user.Surname + "_" + bankAccount.Type.ToString();
                }

                foreach (var request in requests)
                {
                    if (request.Type != RequestType.DeleteUser)
                    {
                        BankAccount bankAccount = bankAccounts.SingleOrDefault(b => b.Id == request.AccountId);
                        
                        if (bankAccount is not null)
                        {
                            request.AccountBalance = bankAccount.Balance;
                            request.AccountCredit = bankAccount.CreditBalance;
                            
                            if (request.Type == RequestType.AddMoney)
                            {
                                request.FinallBalance = bankAccount.Balance + request.AmountBalance;
                                request.FinallCredit = bankAccount.CreditBalance;
                            }
                            else if (request.Type == RequestType.AddCredit)
                            {
                                request.FinallCredit = bankAccount.CreditBalance + request.AmountCredit;
                                request.FinallBalance = bankAccount.Balance + request.AmountCredit;
                            }
                            else if (request.Type == RequestType.DeleteAccount)
                            {
                                request.FinallCredit = bankAccount.CreditBalance;
                                request.FinallBalance = bankAccount.Balance;
                            }
                        }
                        _context.Requests.Update(request);
                    }
                }

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Message = e.Message;
            response.StatusCode = 500;
        }

        return response;
    }


}