using System.Security.Claims;
using AutoMapper;
using CashFlow.Data;
using CashFlow.Dtos.BankAccount;
using CashFlow.Models;

namespace CashFlow.Services.BankAccountServices;

public class BankAccountService : IBankAccountService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BankAccountService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    
    // Helper method to extract the current user's ID from the claims
    private int GetUserId()
    {
        return int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    // Helper method to get the authorization level of the current user
    private async Task<int> GetUserAuthLvl()
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
        return user is null ? -1 : (int)user.AuthorizationLevel;
    }
    
    public Task<ServiceResponse<List<GetBankAccountDto>>> GetAllBankAccounts()
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<List<GetBankAccountDto>>> GetAllWithinUser()
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<GetBankAccountDto>> GetBankAccountById(int id)
    {
        var response = new ServiceResponse<GetBankAccountDto>();
        try
        {
            var bankAccount = await _context.BankAccounts.FirstOrDefaultAsync(b => b.Id == id);
            //Check if bank account exists
            if (bankAccount is null)
            {
                response.Success = false;
                response.StatusCode = 404;
                response.Message = "Not found";
                return response;
            }

            // Check if its user bank account/or user have high permission level
            if (bankAccount.UserId != GetUserId() && !(await GetUserAuthLvl() > (int)AuthorizationLevel.User))
            {
                response.Success = false;
                response.StatusCode = 401;
                response.Message = "Unauthorized";
                return response;
            }

            response.Data = _mapper.Map<GetBankAccountDto>(bankAccount);
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.StatusCode = 500;
            response.Message = ex.Message;
        }

        return response;
    }

    public async Task<ServiceResponse<GetBankAccountDto>> CreateBankAccount(AddBankAccountDto addBankAccountDto)
    {
        var response = new ServiceResponse<GetBankAccountDto>();
        try
        {
            var bankAccount = _mapper.Map<BankAccount>(addBankAccountDto);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            // Check if logged in user exist in DB
            if (user is null)
            {
                response.Success = false;
                response.Message = "Not found";
                response.StatusCode = 404;
                return response;
            }
            
            // Check if this type of an account actually exists
            if ((int)addBankAccountDto.Type <= 0 || (int)addBankAccountDto.Type > (int)Enum
                    .GetValues(typeof(BankAccountType))
                    .Cast<BankAccountType>().Max())
            {
                response.Success = false;
                response.Message = "Wrong bank account type";
                response.StatusCode = 400;
                return response;
            }
            
            //Checks if user already have this account type
            var rempBankAccount = await _context.BankAccounts.FirstOrDefaultAsync(b => b.UserId == GetUserId() 
                && b.Type == addBankAccountDto.Type);
            if (rempBankAccount is not null)
            {
                response.Success = false;
                response.Message = "Only one account of one type for each user";
                response.StatusCode = 409;
                return response;
            }
            else
            {
                bankAccount.UserId = GetUserId();
                bankAccount.Name = user.Name + "_" + user.Surname + "_" + bankAccount.Type.ToString();
                _context.BankAccounts.Add(bankAccount);
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetBankAccountDto>(bankAccount);
            }
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public Task<ServiceResponse<GetBankAccountDto>> UpdateBankAccount(UpdateBankAccountDto updateBankAccountDto)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<List<GetBankAccountDto>>> DeleteBankAccount(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<GetBankAccountDto>> AddBalance(int id, decimal amount)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<GetBankAccountDto>> SubtractBalance(int id, decimal amount)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<GetBankAccountDto>> TransferBalance(int id, int targetId, decimal amount)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<GetBankAccountDto>> AddCredit(int id, decimal amount)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<GetBankAccountDto>> SubtractCredit(int id, decimal amount)
    {
        throw new NotImplementedException();
    }
}