using System.Security.Claims;
using AutoMapper;
using CashFlow.Data;
using CashFlow.Dtos.BankAccount;
using CashFlow.Dtos.Request;
using CashFlow.Models;
using CashFlow.Services.RequestServices;

namespace CashFlow.Services.BankAccountServices;

public class BankAccountService : IBankAccountService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRequestService _requestService;

    public BankAccountService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor, IRequestService requestService)
    {
        _mapper = mapper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _requestService = requestService;
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
    
    public async Task<ServiceResponse<List<GetBankAccountDto>>> GetAll()
    {
        var response = new ServiceResponse<List<GetBankAccountDto>>();
        try
        {
            if (!(await GetUserAuthLvl() > (int)AuthorizationLevel.User))
            {
                response.Success = false;
                response.StatusCode = 401;
                response.Message = "Unauthorized";
                return response;
            }
            
            response.Data = await _context.BankAccounts.Select(b => _mapper.Map<GetBankAccountDto>(b)).ToListAsync();
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Message = e.Message;
            response.StatusCode = 500;
        }

        return response;
    }

    public async Task<ServiceResponse<List<GetBankAccountDto>>> GetAllWithinUser()
    {
        var response = new ServiceResponse<List<GetBankAccountDto>>();
        try
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            //Check if user is in Db
            if (user is null)
            {
                response.Success = false;
                response.Message = "User not found";
                response.StatusCode = 404;
                return response;
            }

            response.Data = await _context.BankAccounts.Where(b => b.UserId == GetUserId()).Select(b => _mapper.Map<GetBankAccountDto>(b)).ToListAsync();
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Message = e.Message;
            response.StatusCode = 500;
        }

        return response;
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
            var tempBankAccount = await _context.BankAccounts.FirstOrDefaultAsync(b => b.UserId == GetUserId() 
                && b.Type == addBankAccountDto.Type);
            if (tempBankAccount is not null)
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

    public async Task<ServiceResponse<GetBankAccountDto>> UpdateBankAccount(UpdateBankAccountDto updateBankAccountDto)
    {
        var response = new ServiceResponse<GetBankAccountDto>();
        try
        {
            //Checks auth level. Only admins can perform account update
            if (!(await GetUserAuthLvl() > (int)AuthorizationLevel.User))
            {
                response.Success = false;
                response.Message = "Unauthorized";
                response.StatusCode = 401;
                return response;
            }
            
            //Check if accounts exists
            var bankAccount = await _context.BankAccounts.FirstOrDefaultAsync(b => b.Id == updateBankAccountDto.Id);
            if (bankAccount is null)
            {
                response.Success = false;
                response.Message = "Not found";
                response.StatusCode = 404;
                return response;
            }

            //Check for conditions regarding bank account types
            if (updateBankAccountDto.Type != bankAccount.Type)
            {
                //Check for condition that user can only have one account of each type
                var tempBankAccount = await _context.BankAccounts
                    .FirstOrDefaultAsync(b => b.Id == updateBankAccountDto.Id);
                User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == tempBankAccount!.UserId);
                tempBankAccount = await _context.BankAccounts.FirstOrDefaultAsync(b =>
                    b.UserId == user!.Id && b.Type == updateBankAccountDto.Type);
                if (tempBankAccount is not null)
                {
                    response.Success = false;
                    response.Message = "Only one account of one type for each user";
                    response.StatusCode = 409;
                    return response;
                }
                // Check if this type of an account actually exists
                if ((int)updateBankAccountDto.Type <= 0 || (int)updateBankAccountDto.Type > (int)Enum
                        .GetValues(typeof(BankAccountType))
                        .Cast<BankAccountType>().Max())
                {
                    response.Success = false;
                    response.Message = "Wrong bank account type";
                    response.StatusCode = 400;
                    return response;
                }
            }

            bankAccount.Type = updateBankAccountDto.Type;
            bankAccount.Balance = updateBankAccountDto.Balance;
            bankAccount.CreditBalance = updateBankAccountDto.CreditBalance;
            bankAccount.UpdatedAt = DateTime.Now;
            //_context.BankAccounts.Update(bankAccount);
            await _context.SaveChangesAsync();
            response.Data = _mapper.Map<GetBankAccountDto>(bankAccount);
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Message = e.Message;
            response.StatusCode = 500;
        }

        return response;
    }

    public async Task<ServiceResponse<List<GetBankAccountDto>>> DeleteBankAccount(int id)
    {
        var response = new ServiceResponse<List<GetBankAccountDto>>();
        try
        {
            var bankAccount = await _context.BankAccounts.FirstOrDefaultAsync(b => b.Id == id);
            // Checks if account exists 
            if (bankAccount is null)
            {
                response.Success = false;
                response.Message = "Not found";
                response.StatusCode = 404;
                return response;
            }
            // How methods works for auth > user (do not create requests)
            if(await GetUserAuthLvl() > (int)AuthorizationLevel.User)
            {
                _context.BankAccounts.Remove(bankAccount);
                await _context.SaveChangesAsync();

                response.Data = await _context.BankAccounts.Select(b => _mapper.Map<GetBankAccountDto>(b)).ToListAsync();
                return response;
            }
            else
            {
                //Checks if account is connected to logged in user
                if (bankAccount.UserId != GetUserId())
                {
                    response.Message = "Unauthorized";
                    response.StatusCode = 401;
                    response.Success = false;
                    return response;
                }
                else
                {
                    AddRequestDto addRequestDto = new AddRequestDto
                    {
                        Type = RequestType.DeleteAccount,
                        AccountId = id,
                        AmountBalance = bankAccount.Balance,
                        AmountCredit = bankAccount.CreditBalance
                    };
                    var tempResponse = await _requestService.CreateRequest(addRequestDto);
                    response.Message = tempResponse.Message != string.Empty ? tempResponse.Message : "Request created";
                    response.Success = tempResponse.Success;
                    response.StatusCode = tempResponse.StatusCode;
                }
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