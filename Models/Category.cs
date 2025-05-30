using System;

namespace ExpenseTrackerAPI.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation property for related expenses
    public List<Expense> Expenses { get; set; } = new List<Expense>();
}
