using System;
using ExpenseTrackerAPI.DTOs.ExpenseDTOs;

namespace ExpenseTrackerAPI.Services.ExpenseServices;

public interface IExpenseServices
{
    Task<IEnumerable<ExpenseDTO>> GetAllExpensesAsync();
    Task<ExpenseDTO> GetExpenseByIdAsync(int id);
    Task<ExpenseDTO> CreateExpenseAsync(CreateExpenseDTO expense);
    Task<ExpenseDTO> UpdateExpenseAsync(int id, UpdateExpenseDTO expense);
    Task<bool> DeleteExpenseAsync(int id);
}
