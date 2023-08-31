using CashFlow.Models;

namespace CashFlow.Services.UpdateServices;

public interface IUpdateService
{
    Task<ServiceResponse<string>> UpdateAll();
}