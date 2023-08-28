using System.Security.Claims;
using AutoMapper;
using CashFlow.Data;
using CashFlow.Dtos.Account;
using CashFlow.Dtos.User;
using CashFlow.Models;
using CashFlow.Services.AuthServices;

namespace CashFlow.Services.UserServices;

public class UserService : IUserService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }
    
    private int GetUserId()
    {
        return int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    private async Task<int> GetUserAuthLvl()
    {
        User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
        return user is null ? -1 : (int)user.AuthorizationLevel;
    }
    
    public async Task<ServiceResponse<List<GetUserDto>>> GetAllUsers()
    {
        var response = new ServiceResponse<List<GetUserDto>>();
        try
        {
            User user = (await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId()))!;
            if ((int)user.AuthorizationLevel > (int)AuthorizationLevel.User)
            {
                response.Data = await _context.Users.Select(u => _mapper.Map<GetUserDto>(u)).ToListAsync();
                response.Message = ((int)AuthorizationLevel.User).ToString();
                return response;
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

    public async Task<ServiceResponse<GetUserDto>> GetUserById(int id)
    {
        var response = new ServiceResponse<GetUserDto>();
        try
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            User? currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            if (user != null)
            {
                if (GetUserId() == user.Id)
                {
                    response.Data = _mapper.Map<GetUserDto>(user);
                    response.Message = ((int)AuthorizationLevel.User).ToString();
                    return response;
                }
                else if (currentUser != null && (int)currentUser.AuthorizationLevel > (int)AuthorizationLevel.User)
                {
                    response.Data = _mapper.Map<GetUserDto>(user);
                    response.Message = ((int)AuthorizationLevel.User).ToString();
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Unauthorized";
                    response.StatusCode = 401;
                }
            }
            else
            {
                response.Success = false;
                response.Message = "User not found";
                response.StatusCode = 404;
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

    public async Task<ServiceResponse<GetUserDto>> UpdateUserEmail(UpdateUserEmailDto updatedUserEmail)
    {
        var response = new ServiceResponse<GetUserDto>();
        try
        {
            if (await GetUserAuthLvl() > (int)AuthorizationLevel.User || updatedUserEmail.Id == GetUserId())
            {
                User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == updatedUserEmail.Id);
                if (user is null)
                {
                    response.Success = false;
                    response.Message = "User not found";
                    response.StatusCode = 404;
                    return response;
                }
                if (!SyntaxChecker.IsValidEmail(updatedUserEmail.Email) && updatedUserEmail.Email != user.Email && updatedUserEmail.Email != string.Empty)
                {
                    response.Success = false;
                    response.Message = "Invalid email";
                    response.StatusCode = 400;
                }
                else if(await _context.Users.FirstOrDefaultAsync(u => u.Email == updatedUserEmail.Email) != null)
                {
                    response.Success = false;
                    response.Message = "Email already in use";
                    response.StatusCode = 400;
                }
                else
                {
                    user.Email = updatedUserEmail.Email;
                }

                await _context.SaveChangesAsync();
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

    public async Task<ServiceResponse<GetUserDto>> UpdateUserNames(UpdateUserNamesDto updateUserNamesDto)
    {
        var response = new ServiceResponse<GetUserDto>();
        try
        {
            if (await GetUserAuthLvl() > (int)AuthorizationLevel.User || updateUserNamesDto.Id == GetUserId())
            {
                User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == updateUserNamesDto.Id);
                if (user is null)
                {
                    response.Success = false;
                    response.Message = "User not found";
                    response.StatusCode = 404;
                    return response;
                }
                if (!SyntaxChecker.IsValidName(updateUserNamesDto.Name) && updateUserNamesDto.Name != user.Name && updateUserNamesDto.Name != string.Empty)
                {
                    response.Success = false;
                    response.Message = "Invalid name";
                    response.StatusCode = 400;
                }
                if (!SyntaxChecker.IsValidName(updateUserNamesDto.Surname) && updateUserNamesDto.Surname != user.Surname && updateUserNamesDto.Surname != string.Empty)
                {
                    response.Success = false;
                    response.Message = response.Message == string.Empty
                        ? "Invalid surname"
                        : response.Message + ", invalid surname";
                    response.StatusCode = 400;
                }
                else
                {
                    user.Name = updateUserNamesDto.Name;
                    user.Surname = updateUserNamesDto.Surname;
                }

                await _context.SaveChangesAsync();
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


    public Task<ServiceResponse<List<GetUserDto>>> DeleteUser(int id)
    {
        throw new NotImplementedException();
    }
}