using System;
using ExpenseTrackerAPI.Data.UnitOfWork;
using ExpenseTrackerAPI.DTOs.CategoryDTOs;
using ExpenseTrackerAPI.Mapping;
using ExpenseTrackerAPI.Repositories.CategoryRepository;

namespace ExpenseTrackerAPI.Services.CategoryServices;

public class CategoryServices : ICategoryServices
{
    private readonly IUnitOfWork _unitOfWork;
    public CategoryServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
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
        var Category = await _unitOfWork.Categories.GetCategoryByIdAsync(id);
        return Categories.EntityToDTO(Category);
    }

    public async Task<CategoryDTO> UpdateCategoryAsync(CreateCategoryDTO category)
    {
        throw new NotImplementedException();
    }
}
