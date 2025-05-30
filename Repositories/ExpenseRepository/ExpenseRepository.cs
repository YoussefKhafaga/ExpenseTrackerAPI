using System;
using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerAPI.Repositories.ExpenseRepository;

public class ExpenseRepository : IExpenseRepository
{
    private readonly ExpenseDBContext _context;

    public ExpenseRepository(ExpenseDBContext context)
    {
        _context = context;
    }

    public async Task<Expense> CreateExpenseAsync(Expense expense)
    {
        await _context.Expenses.AddAsync(expense);
        return expense;
    }

    public async Task<bool> DeleteExpenseAsync(int id)
    {
        var expense = await _context.Expenses.FindAsync(id);
        if (expense == null)
        {
            return false;
        }
        _context.Expenses.Remove(expense);
        return true;
    }

    public async Task<IEnumerable<Expense>> GetAllExpensesAsync()
    {
        var expenses = await _context.Expenses.ToListAsync();
        if (expenses == null || !expenses.Any())
        {
            return null;
        }
        return expenses;
    }

    public async Task<Expense> GetExpenseByIdAsync(int id)
    {
        var expense = await _context.Expenses.FindAsync(id);
        if (expense == null)
        {
            return null;
        }
        return expense;
    }

    public async Task<Expense> UpdateExpenseAsync(Expense expense)
    {
        _context.Expenses.Update(expense);
        return expense;
    }
}
