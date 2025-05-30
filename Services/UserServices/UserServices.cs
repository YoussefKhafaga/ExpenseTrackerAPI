using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExpenseTrackerAPI.Data.UnitOfWork;
using ExpenseTrackerAPI.DTOs.UserDTOs;
using ExpenseTrackerAPI.Repositories.UserRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Services.UserServices;

public class UserServices : IUserServices
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;

    public UserServices(
        IHttpContextAccessor httpContextAccessor,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IConfiguration configuration,
        IUnitOfWork unitOfWork)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> LoginUserAsync(LoginDTO user)
    {
        var identityUser = await _userManager.FindByNameAsync(user.UserName);
        if (identityUser == null)
        {
            throw new KeyNotFoundException("User not found.");
        }
        var result = await _signInManager.CheckPasswordSignInAsync(identityUser, user.Password, false);
        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException("Invalid username or password.");
        }
        var User = new UserDTO
        {
            UserName = identityUser.UserName,
            Email = identityUser.Email,
            UserId = identityUser.Id,
        };
        var token = await GenerateToken(User);
        return token;
    }

    public async Task<string> GenerateToken(UserDTO user)
    {
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.UserId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var authSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["JWT_KEY"])
        );

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT_ISSUER"],
            audience: _configuration["JWT_AUDIENCE"],
            expires: DateTime.UtcNow.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GetCurrentUserIdAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        Console.WriteLine($"Current User ID: {userId}");
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }
        return userId;
    }

    public async Task<string> RegisterUserAsync(RegisterDTO user)
    {
        if (user == null)
        {
            throw new ArgumentException("User registration data cannot be null.");
        }
        var newUser = new User
        {
            UserName = user.UserName,
            Email = user.Email
        };

        var result = await _userManager.CreateAsync(newUser, user.Password);
        if (!result.Succeeded)
        {
            throw new ArgumentException("User registration failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        var User = new UserDTO
        {
            UserName = newUser.UserName,
            Email = newUser.Email,
            UserId = newUser.Id,
        };
        var token = await GenerateToken(User);
        return token;
    }
}
