using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.DTOs.ExpenseDTOs;

public record class UpdateExpenseDTO
{
    [Required, StringLength(20, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;
    [Required, StringLength(100, MinimumLength = 3)]
    public string Description { get; set; } = string.Empty;

    [Required, Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }

    [Required, DataType(DataType.DateTime)]
    public DateTime UpdateDate { get; set; } = DateTime.UtcNow;

    [Required]
    public int CategoryId { get; set; }
}
