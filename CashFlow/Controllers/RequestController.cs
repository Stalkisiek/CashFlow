using CashFlow.Dtos.Request;
using CashFlow.Models;
using CashFlow.Services.RequestServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Controllers;

[Authorize]
[ApiController]
[Route("[controller]s")]
public class RequestController : ControllerBase
{
    private readonly IRequestService _requestService;

    public RequestController(IRequestService requestService)
    {
        _requestService = requestService;
    }
    
    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<GetRequestDto>>>> GetAll()
    {
        var response = await _requestService.GetAll();
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpGet("{userId:int}")]
    public async Task<ActionResult<ServiceResponse<List<GetRequestDto>>>> GetAllByUserId(int userId, bool showOnlyPending)
    {
        var response = await _requestService.GetAllWithinUser(userId, showOnlyPending);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<GetRequestDto>>> CreateRequest(AddRequestDto addRequestDto)
    {
        var response = await _requestService.CreateRequest(addRequestDto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("fulfill")]
    public async Task<ActionResult<ServiceResponse<int>>> FulfillRequest(FulfillRequestDto fulfillRequestDto)
    {
        var response = await _requestService.Fulfill(fulfillRequestDto);
        return StatusCode(response.StatusCode, response);
    }
        
}