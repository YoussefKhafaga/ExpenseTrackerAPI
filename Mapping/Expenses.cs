using System;
using ExpenseTrackerAPI.DTOs.ExpenseDTOs;

namespace ExpenseTrackerAPI.Mapping;
using ExpenseTrackerAPI.Models;

public class Expenses
{
    public static ExpenseDTO ToDTO(Expense expense)
    {
        return new ExpenseDTO
        {
            Id = expense.Id,
            Title = expense.Title,
            Description = expense.Description,
            Amount = expense.Amount,
            CreateDate = expense.CreatedDate,
            UpdateDate = expense.UpdatedDate,
            CategoryName = expense.Category.Name
        };
    }
    public static Expense CreateDTOToModel(CreateExpenseDTO expenseDTO)
    {
        return new Expense
        {
            Title = expenseDTO.Title,
            Description = expenseDTO.Description,
            Amount = expenseDTO.Amount,
            CreatedDate = expenseDTO.CreateDate,
            CategoryId = expenseDTO.CategoryId,
        };
    }
    public static Expense UpdateDTOToModel(UpdateExpenseDTO expenseDTO)
    {
        return new Expense
        {
            Title = expenseDTO.Title,
            Description = expenseDTO.Description,
            Amount = expenseDTO.Amount,
            CreatedDate = expenseDTO.CreateDate,
            CategoryId = expenseDTO.CategoryId,
        };
    }
}
