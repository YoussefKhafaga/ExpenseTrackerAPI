using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.DTOs.CategoryDTOs;

public record class CategoryDTO
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
}
