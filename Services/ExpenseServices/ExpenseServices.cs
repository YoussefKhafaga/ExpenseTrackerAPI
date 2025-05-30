using System;
using ExpenseTrackerAPI.Data.UnitOfWork;
using ExpenseTrackerAPI.DTOs.ExpenseDTOs;
using ExpenseTrackerAPI.Mapping;
using ExpenseTrackerAPI.Services.UserServices;

namespace ExpenseTrackerAPI.Services.ExpenseServices;

public class ExpenseServices : IExpenseServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserServices _userServices;
    public ExpenseServices(IUnitOfWork unitOfWork, IUserServices userServices)
    {
        _unitOfWork = unitOfWork;
        _userServices = userServices;
    }
    public async Task<ExpenseDTO> CreateExpenseAsync(CreateExpenseDTO expenseDTO)
    {
        if (expenseDTO == null)
        {
            throw new ArgumentNullException(nameof(expenseDTO), "Expense data cannot be null.");
        }
        var userId = await _userServices.GetCurrentUserIdAsync();
        if (userId == null)
        {
            throw new UnauthorizedAccessException("User not authenticated.");
        }
        var expense = Expenses.CreateDTOToModel(expenseDTO);
        expense.UserId = userId;
        var category = await _unitOfWork.Categories.GetCategoryByIdAsync(expense.CategoryId);
        Console.WriteLine($"Category: {category}");
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with ID {expense.CategoryId} not found.");
        }
        var createdExpense = await _unitOfWork.Expenses.CreateExpenseAsync(expense);
        Console.WriteLine($"Created Expense: {createdExpense}");
        if (createdExpense == null)
        {
            throw new Exception("Failed to create expense.");
        }
        await _unitOfWork.SaveChangesAsync();
        return Expenses.ToDTO(createdExpense);
    }

    public async Task<bool> DeleteExpenseAsync(int id)
    {
        var expense = await _unitOfWork.Expenses.GetExpenseByIdAsync(id);
        if (expense.UserId != await _userServices.GetCurrentUserIdAsync())
        {
            throw new UnauthorizedAccessException("You do not have permission to delete this expense.");
        }
        var deleted = await _unitOfWork.Expenses.DeleteExpenseAsync(id);
        if (!deleted)
        {
            throw new KeyNotFoundException($"Expense with ID {id} not found.");
        }
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ExpenseDTO>> GetAllExpensesAsync()
    {
        var expenses = await _unitOfWork.Expenses.GetAllExpensesAsync();
        var currentUser = await _userServices.GetCurrentUserIdAsync();
        expenses = expenses.Where(e => e.UserId == currentUser).ToList();
        if (expenses == null || !expenses.Any())
        {
            throw new KeyNotFoundException("No expenses found.");
        }
        return expenses.Select(Expenses.ToDTO);
    }

    public async Task<ExpenseDTO> GetExpenseByIdAsync(int id)
    {
        var expense = await _unitOfWork.Expenses.GetExpenseByIdAsync(id);
        if (expense == null)
        {
            throw new KeyNotFoundException($"Expense with ID {id} not found.");
        }
        if (expense.UserId != await _userServices.GetCurrentUserIdAsync())
        {
            throw new UnauthorizedAccessException("You do not have permission to view this expense.");
        }
        return Expenses.ToDTO(expense);
    }

    public async Task<ExpenseDTO> UpdateExpenseAsync(int id, UpdateExpenseDTO expenseDTO)
    {
        if (expenseDTO == null)
        {
            throw new ArgumentNullException(nameof(expenseDTO), "Expense data cannot be null.");
        }
        var existingExpense = await _unitOfWork.Expenses.GetExpenseByIdAsync(id);
        if (existingExpense == null)
        {
            throw new KeyNotFoundException($"Expense with ID {id} not found.");
        }
        var expense = Expenses.UpdateDTOToModel(expenseDTO);
        var updatedExpense = await _unitOfWork.Expenses.UpdateExpenseAsync(expense);
        if (updatedExpense == null)
        {
            throw new Exception("Failed to update expense.");
        }
        await _unitOfWork.SaveChangesAsync();
        return Expenses.ToDTO(updatedExpense);
    }
}
