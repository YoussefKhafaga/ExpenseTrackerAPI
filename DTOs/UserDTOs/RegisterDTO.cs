using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.DTOs.UserDTOs;

public record class RegisterDTO
{
    [Required, Length(3, 20)]
    public string UserName { get; set; } = string.Empty;
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}
