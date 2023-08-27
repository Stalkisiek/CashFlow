using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using AutoMapper;
using CashFlow.Data;
using CashFlow.Dtos.Authorization;
using CashFlow.Models;
using Microsoft.IdentityModel.Tokens;

namespace CashFlow.Services.AuthServices;

public class AuthRepository : IAuthRepository
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AuthRepository(DataContext context, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _configuration = configuration;
    }
    
    public async Task<bool> UserExists(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user is not null;
    }
    
    static bool IsValidEmail(string email)
    {
        // Defining a regular expression pattern for a valid email address
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        // Using Regex.IsMatch to check if the email matches the pattern
        return Regex.IsMatch(email, pattern);
    }
    
    public Task<ServiceResponse<string>> Login(LoginUserDto loginUserDto)
    {
        var response = new ServiceResponse<string>();
        try
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == loginUserDto.Email);
            if (user is null)
            {
                response.Success = false;
                response.Message = "User not found";
            }
            else if (!VerifyPasswordHash(loginUserDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password";
            }
            else
            {
                response.Data = CreateToken(user);
            }
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Message = e.Message;
        }

        return Task.FromResult(response);
    }

    public async Task<ServiceResponse<int>> Register(RegisterUserDto registerUserDto)
    {
        var response = new ServiceResponse<int>();
        try
        {
            if (!await UserExists(registerUserDto.Email))
            {
                if (IsValidEmail(registerUserDto.Email))
                {
                    CreatePasswordHash(registerUserDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    var user = _mapper.Map<User>(registerUserDto);
                    user.PasswordHash = passwordHash; // Assign the generated hash
                    user.PasswordSalt = passwordSalt; // Assign the generated salt
                    user.AuthorizationLevel = AuthorizationLevel.User; // Set the authorization level
                    user.CreatedAt = DateTime.Now; // Set the creation date
                    user.UpdatedAt = DateTime.Now; // Set the update date

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    response.Data = user.Id; // Set the newly created user's ID in the response data
                }
                else
                {
                    response.Success = false;
                    response.Message = "Invalid email address";
                }
            }
            else
            {
                response.Success = false;
                response.Message = "User already exists";
            }
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Message = e.Message;
        }

        return response;
    }
    
    private int GetUserId()
    {
        return int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
    
    /////// PASSWORD THINGS ///////
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }
    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }
    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var appSettingToken = _configuration.GetSection("AppSettings:Token").Value;
        if (appSettingToken is null)
        {
            throw new Exception("AppSettings Token is null");
        }

        SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingToken));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = creds
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}