using System;

namespace ExpenseTrackerAPI.Models;

public class Expense
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    public int CategoryId { get; set; }
    public string? UserId { get; set; }

    // Navigation properties
    public Category? Category { get; set; }
    public User? User { get; set; }
}
