using System.Security.Claims;
using AutoMapper;
using CashFlow.Data;
using CashFlow.Dtos.Request;
using CashFlow.Models;
using CashFlow.Services.UpdateServices;

namespace CashFlow.Services.RequestServices;

public class RequestService : IRequestService
{
    // Fields to hold necessary dependencies
    private readonly DataContext _context; // Database context
    private readonly IMapper _mapper; // AutoMapper for object mapping
    private readonly IHttpContextAccessor _httpContextAccessor; // Access to HTTP context
    private readonly IUpdateService _updateService;

    // Constructor to initialize dependencies through dependency injection
    public RequestService(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUpdateService updateService)
    {
        _context = context;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _updateService = updateService;
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
    
    public async Task<ServiceResponse<List<GetRequestDto>>> GetAll()
    {
        var response = new ServiceResponse<List<GetRequestDto>>();
        try
        {
            if (await GetUserAuthLvl() > (int)AuthorizationLevel.User)
            {
                response.Data = await _context.Requests.Select(r => _mapper.Map<GetRequestDto>(r)).ToListAsync();
            }
            else
            {
                response.Success = false;
                response.Message = "Unauthorized";
                response.StatusCode = 401;
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

    public async Task<ServiceResponse<List<GetPreviousRequestDto>>> GetAllWithinUser(int id, bool showOnlyPending) // if show all include accepted
    {
        var response = new ServiceResponse<List<GetPreviousRequestDto>>();
        try
        {
            if (await GetUserAuthLvl() > (int)AuthorizationLevel.User || GetUserId() == id)
            {
                if (showOnlyPending)
                {
                    response.Data = await _context.PreviousRequests.Where(r => r.UserId == id)
                        .Where(r => r.Status == RequestAcceptMode.Pending)
                        .Select(r => _mapper.Map<GetPreviousRequestDto>(r)).ToListAsync();
                }
                else
                {
                    response.Data = await _context.PreviousRequests.Where(r => r.UserId == id)
                        .Select(r => _mapper.Map<GetPreviousRequestDto>(r)).ToListAsync();
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Unauthorized";  
                response.StatusCode = 401;
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

    public async Task<ServiceResponse<GetRequestDto>> CreateRequest(AddRequestDto addRequestDto)
    {
        var response = new ServiceResponse<GetRequestDto>();
        try
        {
            if (!((int)addRequestDto.Type > 0 && (int)addRequestDto.Type <= (int)Enum
                    .GetValues(typeof(RequestType))
                    .Cast<RequestType>().Max()))
            {
                response.Message = "Bad request";
                response.Success = false;
                response.StatusCode = 400;
            }
            Request request = _mapper.Map<Request>(addRequestDto);
            //check if there is already a request of the same type
            if (await _context.Requests.FirstOrDefaultAsync(r => r.Type == request.Type && r.UserId == GetUserId()) != null)
            {
                response.Message = "Request already exists";
                response.Success = false;
                response.StatusCode = 400;
                return response;
            }
            if (addRequestDto.Type == RequestType.DeleteUser) // Delete user handler
            {
                request.UserId = GetUserId();
                _context.Requests.Add(request);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetRequestDto>(request);
            }
            else if(addRequestDto.Type == RequestType.DeleteAccount)
            {
                request.UserId = GetUserId();
                _context.Requests.Add(request);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetRequestDto>(request);
            }
            else if(addRequestDto.Type == RequestType.AddMoney)
            {
                request.UserId = GetUserId();
                _context.Requests.Add(request);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetRequestDto>(request);
            }
            else if(addRequestDto.Type == RequestType.AddCredit)
            {
                request.UserId = GetUserId();
                _context.Requests.Add(request);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetRequestDto>(request);
            }
            // Adding request to History database 
            PreviousRequest previousRequest = new PreviousRequest
            {
                Status = RequestAcceptMode.Pending,
                RequestId = request.Id,
                UserId = GetUserId(),
                Type = request.Type
            };
                
            _context.PreviousRequests.Add(previousRequest);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Message = e.Message;
            response.StatusCode = 500;
        }

        return response;
    }

    public async Task<ServiceResponse<int>> Fulfill(FulfillRequestDto fulfillRequestDto)
    {
        var response = new ServiceResponse<int>();
        try
        {
            if (!(await GetUserAuthLvl() > (int)AuthorizationLevel.User))
            {
                response.Success = false;
                response.Message = "Unauthorized";
                response.StatusCode = 401;
                return response;
            }
        
            var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == fulfillRequestDto.Id);
            if (request == null)
            {
                response.Success = false;
                response.StatusCode = 404;
                response.Message = "Request not found";
                return response;
            }

            PreviousRequest? previousRequest =
                await _context.PreviousRequests.FirstOrDefaultAsync(p => p.RequestId == request.Id);
            if (previousRequest is null)
            {
                response.Success = false;
                response.StatusCode = 404;
                response.Message = "Request history not found";
                return response;
            }

            previousRequest.Message = fulfillRequestDto.Message;
            if (fulfillRequestDto.Accepted == false)
            {
                response.Success = false;
                response.StatusCode = 200;
                response.Message = "Rejected";
                
                previousRequest.Status = RequestAcceptMode.Rejected;
                _context.Requests.Remove(request);
                await _context.SaveChangesAsync();
                await _updateService.UpdateAll();
                return response;
            }

            if (request.Type == RequestType.DeleteUser) // DeleteUser handler
            {
                User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
                response.Data = request.Id;
                if (user is null) // Deletes request when user is no longer in DB
                {
                    response.Success = false;
                    response.StatusCode = 404;
                    response.Message = "Not found";

                    previousRequest.Message = "User was already deleted";
                    previousRequest.Status = RequestAcceptMode.Deleted;
                    _context.Requests.Remove(request);
                    await _context.SaveChangesAsync();
                    await _updateService.UpdateAll();
                    return response;
                }
                else
                {
                    previousRequest.Status = RequestAcceptMode.Accepted;
                    response.Message = "Accepted";
                    _context.Users.Remove(user);
                    _context.Requests.Remove(request);
                    await _context.SaveChangesAsync();
                    await _updateService.UpdateAll();
                    return response;
                }
                
            } 
            else if (request.Type == RequestType.DeleteAccount) // Delete Account handler
            {
                BankAccount? bankAccount =
                    await _context.BankAccounts.FirstOrDefaultAsync(b => b.Id == request.AccountId);
                response.Data = request.Id;
                if (bankAccount is null)
                {
                    response.Success = false;
                    response.StatusCode = 404;
                    response.Message = "Not found";
                    
                    previousRequest.Message = "Account was already deleted";
                    previousRequest.Status = RequestAcceptMode.Deleted;
                    _context.Requests.Remove(request);
                    await _context.SaveChangesAsync();
                    await _updateService.UpdateAll();
                    return response;
                }
                else
                {
                    previousRequest.Status = RequestAcceptMode.Accepted;
                    response.Message = "Accepted";
                    _context.BankAccounts.Remove(bankAccount);
                    _context.Requests.Remove(request);
                    await _context.SaveChangesAsync();
                    await _updateService.UpdateAll();
                    return response;
                }
            }
            else if (request.Type == RequestType.AddMoney) // Delete Account handler
            {
                BankAccount? bankAccount =
                    await _context.BankAccounts.FirstOrDefaultAsync(b => b.Id == request.AccountId);
                response.Data = request.Id;
                if (bankAccount is null)
                {
                    response.Success = false;
                    response.StatusCode = 404;
                    response.Message = "Not found";
                    
                    previousRequest.Message = "Account doesnt exist";
                    previousRequest.Status = RequestAcceptMode.Deleted;
                    _context.Requests.Remove(request);
                    await _context.SaveChangesAsync();
                    await _updateService.UpdateAll();
                    return response;
                }
                else
                {
                    previousRequest.Status = RequestAcceptMode.Accepted;
                    response.Message = "Accepted";
                    
                    bankAccount.Balance += request.AmountBalance;
                    
                    _context.Requests.Remove(request);
                    await _context.SaveChangesAsync();
                    await _updateService.UpdateAll();
                    return response;
                }
            }
            else if (request.Type == RequestType.AddCredit) // Delete Account handler
            {
                BankAccount? bankAccount =
                    await _context.BankAccounts.FirstOrDefaultAsync(b => b.Id == request.AccountId);
                response.Data = request.Id;
                if (bankAccount is null)
                {
                    response.Success = false;
                    response.StatusCode = 404;
                    response.Message = "Not found";
                    
                    previousRequest.Message = "Account doesnt exist";
                    previousRequest.Status = RequestAcceptMode.Deleted;
                    _context.Requests.Remove(request);
                    await _context.SaveChangesAsync();
                    await _updateService.UpdateAll();
                    return response;
                }
                else
                {
                    previousRequest.Status = RequestAcceptMode.Accepted;
                    response.Message = "Accepted";

                    bankAccount.CreditBalance += request.AmountCredit;
                    bankAccount.Balance += request.AmountCredit;
                    
                    _context.Requests.Remove(request);
                    await _context.SaveChangesAsync();
                    await _updateService.UpdateAll();
                    return response;
                }
            }
            response.Data = request.Id;
            previousRequest.Status = RequestAcceptMode.Accepted;
            response.Message = "Accepted";
            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();
            await _updateService.UpdateAll();
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