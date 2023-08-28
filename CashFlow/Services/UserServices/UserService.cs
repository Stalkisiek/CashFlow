using System.Security.Claims;
using AutoMapper;
using CashFlow.Data;
using CashFlow.Dtos.Account;
using CashFlow.Dtos.Authorization;
using CashFlow.Dtos.User;
using CashFlow.Models;
using CashFlow.Services.AuthServices;

namespace CashFlow.Services.UserServices;

public class UserService : IUserService
{
    // Fields to hold necessary dependencies
    private readonly DataContext _context; // Database context
    private readonly IMapper _mapper; // AutoMapper for object mapping
    private readonly IHttpContextAccessor _httpContextAccessor; // Access to HTTP context

    // Constructor to initialize dependencies through dependency injection
    public UserService(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
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

    // Method to retrieve a list of all users, subject to authorization level
    public async Task<ServiceResponse<List<GetUserDto>>> GetAllUsers()
    {
        var response = new ServiceResponse<List<GetUserDto>>();
        try
        {
            // Retrieve the current user
            var user = (await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId()))!;

            // Check if the user has sufficient authorization level
            if ((int)user.AuthorizationLevel > (int)AuthorizationLevel.User)
            {
                // Map all users to GetUserDto and return the list
                response.Data = await _context.Users.Select(u => _mapper.Map<GetUserDto>(u)).ToListAsync();
                response.Message = ((int)AuthorizationLevel.User).ToString();
                return response;
            }
            else
            {
                // Unauthorized user
                response.Success = false;
                response.Message = "Unauthorized";
                response.StatusCode = 401;
            }
        }
        catch (Exception e)
        {
            // Internal server error
            response.Success = false;
            response.Message = e.Message;
            response.StatusCode = 500;
        }

        return response;
    }

    // Method to retrieve a user by their ID, subject to authorization level
    public async Task<ServiceResponse<GetUserDto>> GetUserById(int id)
    {
        var response = new ServiceResponse<GetUserDto>();
        try
        {
            // Retrieve the requested user and the current user
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

            if (user != null)
            {
                // Check if the requester has the necessary authorization
                if (GetUserId() == user.Id || (currentUser != null &&
                                               (int)currentUser.AuthorizationLevel > (int)AuthorizationLevel.User))
                {
                    // Map the user to GetUserDto and return
                    response.Data = _mapper.Map<GetUserDto>(user);
                    response.Message = ((int)AuthorizationLevel.User).ToString();
                    return response;
                }
                else
                {
                    // Unauthorized user
                    response.Success = false;
                    response.Message = "Unauthorized";
                    response.StatusCode = 401;
                }
            }
            else
            {
                // User not found
                response.Success = false;
                response.Message = "User not found";
                response.StatusCode = 404;
            }
        }
        catch (Exception e)
        {
            // Internal server error
            response.Success = false;
            response.Message = e.Message;
            response.StatusCode = 500;
        }

        return response;
    }

    // Method to update a user's email, subject to authorization level
    public async Task<ServiceResponse<GetUserDto>> UpdateUserEmail(UpdateUserEmailDto updatedUserEmail)
    {
        var response = new ServiceResponse<GetUserDto>();
        try
        {
            // Retrieve the user to update
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == updatedUserEmail.Id);
            if (user is null)
            {
                // User not found
                response.Success = false;
                response.Message = "User not found";
                response.StatusCode = 404;
                return response;
            }

            // Check authorization level
            if ((await GetUserAuthLvl() > (int)AuthorizationLevel.User &&
                 await GetUserAuthLvl() > (int)user.AuthorizationLevel) ||
                updatedUserEmail.Id == GetUserId())
            {
                // Check and update email based on validity and availability
                if (!SyntaxChecker.IsValidEmail(updatedUserEmail.Email) && updatedUserEmail.Email != user.Email &&
                    updatedUserEmail.Email != string.Empty)
                {
                    response.Success = false;
                    response.Message = "Invalid email";
                    response.StatusCode = 400;
                }
                else if (await _context.Users.FirstOrDefaultAsync(u => u.Email == updatedUserEmail.Email) != null)
                {
                    response.Success = false;
                    response.Message = "Email already in use";
                    response.StatusCode = 400;
                }
                else
                {
                    user.Email = updatedUserEmail.Email;
                    response.Data = _mapper.Map<GetUserDto>(user);
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                // Unauthorized user
                response.Success = false;
                response.Message = "Unauthorized";
                response.StatusCode = 401;
            }
        }
        catch (Exception e)
        {
            // Internal server error
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
            // Fetch the user to update by their ID
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == updateUserNamesDto.Id);
            if (user is null)
            {
                // User not found
                response.Success = false;
                response.Message = "User not found";
                response.StatusCode = 404;
                return response;
            }

            // Check if the requester has sufficient authorization level
            if ((await GetUserAuthLvl() > (int)AuthorizationLevel.User &&
                 await GetUserAuthLvl() > (int)user.AuthorizationLevel) ||
                updateUserNamesDto.Id == GetUserId())
            {
                // Check and update user's name
                if (!SyntaxChecker.IsValidName(updateUserNamesDto.Name) && updateUserNamesDto.Name != user.Name &&
                    updateUserNamesDto.Name != string.Empty)
                {
                    // Invalid name provided
                    response.Success = false;
                    response.Message = "Invalid name";
                    response.StatusCode = 400;
                }

                // Check and update user's surname
                if (!SyntaxChecker.IsValidName(updateUserNamesDto.Surname) &&
                    updateUserNamesDto.Surname != user.Surname && updateUserNamesDto.Surname != string.Empty)
                {
                    // Invalid surname provided
                    response.Success = false;
                    response.Message = response.Message == string.Empty
                        ? "Invalid surname"
                        : response.Message + ", invalid surname";
                    response.StatusCode = 400;
                }
                else
                {
                    // Update user's name and surname
                    user.Name = updateUserNamesDto.Name;
                    user.Surname = updateUserNamesDto.Surname;
                    response.Data = _mapper.Map<GetUserDto>(user);
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                // Unauthorized user
                response.Success = false;
                response.Message = "Unauthorized";
                response.StatusCode = 401;
            }
        }
        catch (Exception e)
        {
            // Internal server error
            response.Success = false;
            response.Message = e.Message;
            response.StatusCode = 500;
        }

        return response;
    }

    public async Task<ServiceResponse<GetUserDto>> UpdateUserAuthorizationLevel(
        UpdateUserAuthorizationLevelDto updateUserAuthorizationLevelDto)
    {
        var response = new ServiceResponse<GetUserDto>();
        try
        {
            // Check if the requester has sufficient authorization level
            if (await GetUserAuthLvl() > (int)AuthorizationLevel.User)
            {
                // Fetch the user to update by their ID
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == updateUserAuthorizationLevelDto.Id);
                if (user is null)
                {
                    // User not found
                    response.Success = false;
                    response.Message = "User not found";
                    response.StatusCode = 404;
                    return response;
                }

                // Validate the provided authorization level
                if ((int)updateUserAuthorizationLevelDto.AuthorizationLevel < 1 ||
                    (int)updateUserAuthorizationLevelDto.AuthorizationLevel > (int)Enum
                        .GetValues(typeof(AuthorizationLevel))
                        .Cast<AuthorizationLevel>().Max())
                {
                    // Invalid authorization level provided
                    response.Success = false;
                    response.Message = "Invalid authorization level";
                    response.StatusCode = 400;
                }
                else if ((int)updateUserAuthorizationLevelDto.AuthorizationLevel > await GetUserAuthLvl() ||
                         ((int)user.AuthorizationLevel >= await GetUserAuthLvl() && user.Id != GetUserId()))
                {
                    // Unauthorized user
                    response.Success = false;
                    response.Message = "Unauthorized";
                    response.StatusCode = 401;
                }
                else
                {
                    // Update user's authorization level
                    user.AuthorizationLevel = updateUserAuthorizationLevelDto.AuthorizationLevel;
                    response.Data = _mapper.Map<GetUserDto>(user);
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                // Unauthorized user
                response.Success = false;
                response.Message = "Unauthorized";
                response.StatusCode = 401;
            }
        }
        catch (Exception e)
        {
            // Internal server error
            response.Success = false;
            response.Message = e.Message;
            response.StatusCode = 500;
        }

        return response;
    }


    // Method to update a user's password (Not implemented)
    public Task<ServiceResponse<GetUserDto>> UpdateUserPassword(UpdateUserPasswordDto updateUserPasswordDto)
    {
        throw new NotImplementedException();
    }

    // Method to delete a user (Not implemented)
    public Task<ServiceResponse<List<GetUserDto>>> DeleteUser(int id)
    {
        throw new NotImplementedException();
    }
}