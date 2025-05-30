using System;
using ExpenseTrackerAPI.DTOs.UserDTOs;

namespace ExpenseTrackerAPI.Services.UserServices;

public interface IUserServices
{
    Task<string> RegisterUserAsync(RegisterDTO user);
    Task<string> LoginUserAsync(LoginDTO user);
    Task<string> GetCurrentUserIdAsync();
    Task<string> GenerateToken(UserDTO user);
    
}
