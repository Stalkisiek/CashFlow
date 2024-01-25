using System.Security.Claims;
using AutoMapper;
using CashFlow.Data;
using CashFlow.Dtos.Account;
using CashFlow.Dtos.Authorization;
using CashFlow.Dtos.Request;
using CashFlow.Dtos.User;
using CashFlow.Models;
using CashFlow.Services.AuthServices;
using CashFlow.Services.RequestServices;
using CashFlow.Services.UpdateServices;

namespace CashFlow.Services.UserServices;

public class UserService : IUserService
{
    // Fields to hold necessary dependencies
    private readonly DataContext _context; // Database context
    private readonly IMapper _mapper; // AutoMapper for object mapping
    private readonly IHttpContextAccessor _httpContextAccessor; // Access to HTTP context
    private readonly IAuthRepository _authRepository;
    private readonly IRequestService _requestService;
    private readonly IUpdateService _updateService;

    // Constructor to initialize dependencies through dependency injection
    public UserService(DataContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, 
        IAuthRepository authRepository, IRequestService requestService, IUpdateService updateService)
    {
        _context = context;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _authRepository = authRepository;
        _requestService = requestService;
        _updateService = updateService;
    }

    // Helper method to extract the current user's ID from the claims
    private int GetUserId()
    {
        // return int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!); CHANGE
        return 2;
    }

    // Helper method to get the authorization level of the current user
    private async Task<int> GetUserAuthLvl()
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
        return user is null ? -1 : (int)user.AuthorizationLevel;
    }

    // Method to retrieve a list of all users, subject to authorization level
    public async Task<ServiceResponse<List<GetUserDto>>> GetAllUsers(int? id = null, int? authLvl = null, string? name = null, string? surname = null, string? email = null)
    {
        var response = new ServiceResponse<List<GetUserDto>>();
        try
        {
            // Retrieve the current user
            var user = (await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId()))!;

            // Check if the user has sufficient authorization level
            if (true || (int)user.AuthorizationLevel > (int)AuthorizationLevel.User || true) // CHANGE
            {
                var query = _context.Users.AsQueryable();
                if (id.HasValue)
                {
                    query = query.Where(r => r.Id == id);
                }
                if(authLvl.HasValue)
                {
                    query = query.Where(r => (int)r.AuthorizationLevel == authLvl);
                }
                if(name != null)
                {
                    query = query.Where(r => r.Name == name);
                }
                if(surname != null)
                {
                    query = query.Where(r => r.Surname == surname);
                }
                if(email != null)
                {
                    query = query.Where(r => r.Email == email);
                }

                // Map all users to GetUserDto and return the list
                response.Data = await query.Select(u => _mapper.Map<GetUserDto>(u)).ToListAsync();
                response.Message = "";
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

    // Method to retrieve the current user
    public async Task<ServiceResponse<GetUserDto>> GetCurrentUser()
    {
        var response = new ServiceResponse<GetUserDto>();
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            if (user != null)
            {
                // Map the user to GetUserDto and return
                response.Data = _mapper.Map<GetUserDto>(user);
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
                    user.UpdatedAt = DateTime.Now;
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
                    user.UpdatedAt = DateTime.Now;
                    response.Data = _mapper.Map<GetUserDto>(user);
                }

                await _context.SaveChangesAsync();
                await _updateService.UpdateAll();
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
                    user.UpdatedAt = DateTime.Now;
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
    public async Task<ServiceResponse<GetUserDto>> UpdateUserPassword(UpdateUserPasswordDto updateUserPasswordDto)
    {
        var response = new ServiceResponse<GetUserDto>();
        try
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            if (user is null)
            {
                //User not found
                response.Success = false;
                response.Message = "User not found";
                response.StatusCode = 404;
                return response;
            }

            if (!_authRepository.VerifyPasswordHash(updateUserPasswordDto.CurrentPassword, user.PasswordHash,
                    user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Unauthorized";
                response.StatusCode = 401;
                return response;
            }
            else if (updateUserPasswordDto.CurrentPassword == updateUserPasswordDto.NewPassword)
            {
                response.Success = false;
                response.Message = "New password same as old one";
                response.StatusCode = 400;
                return response;
            }
            else
            {
                _authRepository.CreatePasswordHash(updateUserPasswordDto.NewPassword, 
                    out var passwordHash, out var passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.UpdatedAt = DateTime.Now;
                response.Data = _mapper.Map<GetUserDto>(user);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            //Internal error
            response.Success = false;
            response.StatusCode = 500;
            response.Message = e.Message;
        }

        return response;
    }

    public async Task<ServiceResponse<List<GetUserDto>>> DeleteUserById(int id)
    {
        var response = new ServiceResponse<List<GetUserDto>>();
        try
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is null)
            {
                response.Success = false;
                response.Message = "Not found";
                response.StatusCode = 404;
                return response;
            }
            
            if ((await GetUserAuthLvl() > (int)AuthorizationLevel.User && await GetUserAuthLvl() > (int)user.AuthorizationLevel))
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                response.Data = await _context.Users.Select(u => _mapper.Map<GetUserDto>(u)).ToListAsync();
            }
            else if(id == GetUserId())
            {
                AddRequestDto addRequestDto = new AddRequestDto
                {
                    Type = RequestType.DeleteUser
                };
                
                var temp = await _requestService.CreateRequest(addRequestDto);
                if (temp.StatusCode != 200)
                {
                    response.Success = false;
                    response.Message = temp.Message;
                    response.StatusCode = temp.StatusCode;
                    return response;
                }
                response.Message = "Request created";
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

    // Method to delete a user (Not implemented)
    public async Task<ServiceResponse<string>> DeleteCurrentUser()
    {
        var response = new ServiceResponse<string>
        {
            Data = string.Empty
        };
        try
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            if (user is null)
            {
                response.Success = false;
                response.Message = "Not found";
                response.StatusCode = 404;
                return response;
            }

            if (await GetUserAuthLvl() > (int)AuthorizationLevel.User)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                response.StatusCode = 200;
            }
            else
            {
                AddRequestDto addRequestDto = new AddRequestDto
                {
                    Type = RequestType.DeleteUser
                };
                
                var temp = await _requestService.CreateRequest(addRequestDto);
                if (temp.StatusCode != 200)
                {
                    response.Success = false;
                    response.Message = temp.Message;
                    response.StatusCode = temp.StatusCode;
                    return response;
                }
                response.Message = "Request created";
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
}