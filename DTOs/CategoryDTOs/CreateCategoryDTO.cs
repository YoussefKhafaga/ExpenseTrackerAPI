using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.DTOs.CategoryDTOs;

public record class CreateCategoryDTO
{
    [Required, Length(3, 20)]
    public string Name { get; set; } = string.Empty;
}
