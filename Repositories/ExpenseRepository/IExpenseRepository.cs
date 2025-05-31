using System;
using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Repositories.ExpenseRepository;

public interface IExpenseRepository
{
    Task<IEnumerable<Expense>> GetAllExpensesAsync();   
    Task<Expense> GetExpenseByIdAsync(int id);
    Task<Expense> CreateExpenseAsync(Expense expense);
    Task<Expense> UpdateExpenseAsync(Expense expense);
    Task<bool> DeleteExpenseAsync(int id);
}
