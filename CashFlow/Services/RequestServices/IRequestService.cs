using CashFlow.Dtos.Account;
using CashFlow.Dtos.Request;
using CashFlow.Models;

namespace CashFlow.Services.RequestServices;

public interface IRequestService
{
    Task<ServiceResponse<List<GetRequestDto>>> GetAll();
    Task<ServiceResponse<List<GetPreviousRequestDto>>> GetAllWithinUser(int id);
    Task<ServiceResponse<GetRequestDto>> CreateRequest(AddRequestDto addRequestDto);
    Task<ServiceResponse<int>> Fulfill(FulfillRequestDto fulfillRequestDto);
}