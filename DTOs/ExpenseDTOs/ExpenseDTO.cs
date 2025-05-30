using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.DTOs.ExpenseDTOs;

public record class ExpenseDTO
{
    public int Id { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime CreateDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    
    public string? CategoryName { get; set; }
}
