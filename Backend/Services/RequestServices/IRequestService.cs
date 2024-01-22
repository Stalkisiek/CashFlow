using CashFlow.Dtos.Account;
using CashFlow.Dtos.Request;
using CashFlow.Models;

namespace CashFlow.Services.RequestServices;

public interface IRequestService
{
    Task<ServiceResponse<List<GetRequestDto>>> GetAll(int? userId = null, int? requestId = null, int? requestType = null);
    Task<ServiceResponse<List<GetPreviousRequestDto>>> GetAllWithinUser(int id, bool showOnlyPending);
    Task<ServiceResponse<GetRequestDto>> CreateRequest(AddRequestDto addRequestDto);
    Task<ServiceResponse<int>> Fulfill(FulfillRequestDto fulfillRequestDto);
}