using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.DTOs.UserDTOs;

public record class UserDTO
{
    public string UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
