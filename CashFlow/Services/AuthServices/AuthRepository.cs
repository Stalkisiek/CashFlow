using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using CashFlow.Data;
using CashFlow.Dtos.Authorization;
using CashFlow.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CashFlow.Services.AuthServices;

public class AuthRepository : IAuthRepository
{
    private readonly DataContext _context; // Database context
    private readonly IHttpContextAccessor _httpContextAccessor; // Access to HTTP context
    private readonly IMapper _mapper; // AutoMapper for object mapping
    private readonly IConfiguration _configuration; // Configuration for app settings

    public AuthRepository(DataContext context, IMapper mapper, IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _configuration = configuration;
    }

    // Check if a user with the given email exists in the database
    public async Task<bool> UserExists(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user is not null;
    }

    // Method to handle user login
    public Task<ServiceResponse<string>> Login(LoginUserDto loginUserDto)
    {
        var response = new ServiceResponse<string>();
        try
        {
            // Retrieve the user based on the provided email
            var user = _context.Users.FirstOrDefault(u => u.Email == loginUserDto.Email);
            if (user is null)
            {
                // User not found
                response.Success = false;
                response.Message = "User not found";
                response.StatusCode = 404;
            }
            else if (!VerifyPasswordHash(loginUserDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                // Incorrect password
                response.Success = false;
                response.Message = "Wrong password";
                response.StatusCode = 401;
            }
            else
            {
                // Create and return a JWT token upon successful login
                response.Data = CreateToken(user);
            }
        }
        catch (Exception e)
        {
            // Internal server error
            response.Success = false;
            response.Message = e.Message;
            response.StatusCode = 500;
        }

        return Task.FromResult(response);
    }

    // Method to register a new user
    public async Task<ServiceResponse<int>> Register(RegisterUserDto registerUserDto)
    {
        var response = new ServiceResponse<int>();
        try
        {
            // Check if user with provided email already exists
            if (!await UserExists(registerUserDto.Email))
            {
                // Validate email format using SyntaxChecker and proceed with registration
                if (SyntaxChecker.IsValidEmail(registerUserDto.Email))
                {
                    CreatePasswordHash(registerUserDto.Password, out var passwordHash, out var passwordSalt);
                    var user = _mapper.Map<User>(registerUserDto);
                    if (SyntaxChecker.IsValidName(user.Name) == false || SyntaxChecker.IsValidName(user.Surname) == false)
                    {
                        response.Success = false;
                        response.Message = "Invalid name/surname";
                        response.StatusCode = 400;
                        return response;
                    }
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.AuthorizationLevel = AuthorizationLevel.User;
                    user.CreatedAt = DateTime.Now;
                    user.UpdatedAt = DateTime.Now;

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    response.Data = user.Id; // Newly created user's ID
                }
                else
                {
                    // Invalid email format
                    response.Success = false;
                    response.Message = "Invalid email address";
                    response.StatusCode = 400;
                    return response;
                }
            }
            else
            {
                // User already exists
                response.Success = false;
                response.Message = "User already exists";
                response.StatusCode = 400;
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

    // Helper method to extract the current user's ID from the claims
    private int GetUserId()
    {
        return int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    // Helper methods for handling password operations

    // Method to create a password hash and salt
    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    // Method to verify a password hash and salt
    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }

    // Method to create a JWT token
    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email)
        };

        var appSettingToken = _configuration.GetSection("AppSettings:Token").Value;
        if (appSettingToken is null) throw new Exception("AppSettings Token is null");

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingToken));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}