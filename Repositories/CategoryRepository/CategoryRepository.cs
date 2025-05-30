using System;
using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Repositories.CategoryRepository;

public class CategoryRepository : ICategoryRepository
{
    private readonly ExpenseDBContext _context;
    public CategoryRepository(ExpenseDBContext context)
    {
        _context = context;
    }
    public async Task<Category> CreateCategoryAsync(Category category)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id) 
               ?? null;
    }

    public async Task<Category> UpdateCategoryAsync(Category category)
    {
        throw new NotImplementedException();
    }
}
