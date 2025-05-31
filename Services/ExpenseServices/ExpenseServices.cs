using System;
using ExpenseTrackerAPI.Data.UnitOfWork;
using ExpenseTrackerAPI.DTOs.ExpenseDTOs;
using ExpenseTrackerAPI.Mapping;
using ExpenseTrackerAPI.Services.CategoryServices;
using ExpenseTrackerAPI.Services.UserServices;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpenseTrackerAPI.Services.ExpenseServices;

public class ExpenseServices : IExpenseServices
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserServices _userServices;
    private readonly ICategoryServices _categoryService;
    public ExpenseServices(IUnitOfWork unitOfWork, IUserServices userServices, ICategoryServices categoryService)
    {
        _unitOfWork = unitOfWork;
        _userServices = userServices;
        _categoryService = categoryService;
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
        var category = await _categoryService.GetCategoryByIdAsync(expense.CategoryId);
        if (category == null)
        {
            throw new KeyNotFoundException($"Category with ID {expense.CategoryId} not found.");
        }
        var createdExpense = await _unitOfWork.Expenses.CreateExpenseAsync(expense);
        if (createdExpense == null)
        {
            throw new Exception("Failed to create expense.");
        }
        await _unitOfWork.SaveChangesAsync();
        var CreatedexpenseDTO = Expenses.ToDTO(createdExpense);
        CreatedexpenseDTO.CategoryName = category.Name;
        return CreatedexpenseDTO;
    }

    public async Task<bool> DeleteExpenseAsync(int id)
    {
        var expense = await _unitOfWork.Expenses.GetExpenseByIdAsync(id);
        var currentUserId = await _userServices.GetCurrentUserIdAsync();
        Console.WriteLine($"Here: Expense UserId = {expense.UserId}, Current UserId = {currentUserId}");
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

    public async Task<PagedResponse<ExpenseDTO>> GetAllExpensesAsync(ExpenseQueryParameters queryParams)
    {
        var expenses = await _unitOfWork.Expenses.GetAllExpensesAsync();
        var currentUser = await _userServices.GetCurrentUserIdAsync();

        expenses = expenses.Where(e => e.UserId == currentUser).ToList();

        if (expenses == null || !expenses.Any())
        {
            throw new KeyNotFoundException("No expenses found.");
        }
        Console.WriteLine(expenses.Count());
        var now = DateTime.UtcNow;
        foreach (var fetchedexpense in expenses)
        {
            try
            {
                Console.WriteLine($"Expense ID: {fetchedexpense.Id}, CreatedDate: {fetchedexpense.CreatedDate}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error on Expense ID {fetchedexpense.Id}: {ex.Message}");
            }
        }

        if (!string.IsNullOrEmpty(queryParams.Filter))
        {
            switch (queryParams.Filter.ToLower())
            {
                case "pastweek":
                    expenses = expenses.Where(e => e.CreatedDate >= now.AddDays(-7)).ToList();
                    break;
                case "pastmonth":
                    expenses = expenses.Where(e => e.CreatedDate >= now.AddMonths(-1)).ToList();
                    break;
                case "last3months":
                    expenses = expenses.Where(e => e.CreatedDate >= now.AddMonths(-3)).ToList();
                    break;
                case "custom":
                    if (queryParams.StartDate.HasValue)
                        expenses = expenses.Where(e => e.CreatedDate >= queryParams.StartDate.Value.Date).ToList();
                    if (queryParams.EndDate.HasValue)
                        expenses = expenses.Where(e => e.CreatedDate <= queryParams.EndDate.Value.Date).ToList();
                    break;
                default:
                    // No filtering
                    break;
            }
        }

        var totalItems = expenses.Count();

        // Apply sorting and pagination
        var pagedExpenses = expenses
            .OrderByDescending(e => e.CreatedDate)
            .Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .ToList();

        var expenseDTOs = new List<ExpenseDTO>();
        foreach (var expense in pagedExpenses)
        {
            var category = await _categoryService.GetCategoryByIdAsync(expense.CategoryId);
            var dto = Expenses.ToDTO(expense);
            dto.CategoryName = category.Name;
            expenseDTOs.Add(dto);
        }

        return new PagedResponse<ExpenseDTO>
        {
            Items = expenseDTOs,
            TotalItems = totalItems,
            PageNumber = queryParams.PageNumber,
            PageSize = queryParams.PageSize
        };
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
        var category = await _categoryService.GetCategoryByIdAsync(expense.CategoryId);
        var expenseDTO = Expenses.ToDTO(expense);
        expenseDTO.CategoryName = category.Name;
        return expenseDTO;
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
        var currentUserId = await _userServices.GetCurrentUserIdAsync();
        if (existingExpense.UserId != currentUserId)
        {
            throw new UnauthorizedAccessException("You do not have permission to view this expense.");
        }
        existingExpense.Title = expenseDTO.Title;
        existingExpense.Description = expenseDTO.Description;
        existingExpense.Amount = expenseDTO.Amount;
        existingExpense.CategoryId = expenseDTO.CategoryId;
        existingExpense.UpdatedDate = expenseDTO.UpdateDate;
        var updatedExpense = await _unitOfWork.Expenses.UpdateExpenseAsync(existingExpense);
        if (updatedExpense == null)
        {
            throw new Exception("Failed to update expense.");
        }
        await _unitOfWork.SaveChangesAsync();
        var category = await _categoryService.GetCategoryByIdAsync(existingExpense.CategoryId);
        var UpdatedexpenseDTO = Expenses.ToDTO(existingExpense);
        UpdatedexpenseDTO.CategoryName = category.Name;
        return UpdatedexpenseDTO;
    }

}
