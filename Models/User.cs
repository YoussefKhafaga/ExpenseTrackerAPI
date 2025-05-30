using System;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTrackerAPI.Models;

public class User : IdentityUser
{
    public List<Expense> Expenses { get; set; } = new List<Expense>();
}
