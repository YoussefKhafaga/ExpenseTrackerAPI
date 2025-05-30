using System;
using ExpenseTrackerAPI.DTOs.CategoryDTOs;

namespace ExpenseTrackerAPI.Services.CategoryServices;

public interface ICategoryServices
{
    Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
    Task<CategoryDTO> GetCategoryByIdAsync(int id);
    Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO category);
    Task<CategoryDTO> UpdateCategoryAsync(CreateCategoryDTO category);
    Task<bool> DeleteCategoryAsync(int id);
}
