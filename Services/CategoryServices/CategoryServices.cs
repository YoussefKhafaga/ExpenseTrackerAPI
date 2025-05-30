using System;
using ExpenseTrackerAPI.DTOs.CategoryDTOs;

namespace ExpenseTrackerAPI.Services.CategoryServices;

public class CategoryServices : ICategoryServices
{
    public async Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO category)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<CategoryDTO> UpdateCategoryAsync(CreateCategoryDTO category)
    {
        throw new NotImplementedException();
    }
}
