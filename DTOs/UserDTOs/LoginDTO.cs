using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.DTOs.UserDTOs;

public record class LoginDTO
{
    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
