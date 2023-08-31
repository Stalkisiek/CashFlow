using CashFlow.Models;
using CashFlow.Services.UpdateServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Controllers;

[Authorize]
[ApiController]
[Route("[controller]s")]
public class UpdateController : ControllerBase
{
    private readonly IUpdateService _updateService;

    public UpdateController(IUpdateService updateService)
    {
        _updateService = updateService;
    }

    [HttpPut]
    public async Task<ActionResult<ServiceResponse<string>>> UpdateAll()
    {
        var response = await _updateService.UpdateAll();
        return StatusCode(response.StatusCode, response);
    }
}