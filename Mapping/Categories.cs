using System;
using ExpenseTrackerAPI.DTOs.CategoryDTOs;
using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Mapping;

public class Categories
{
    public static Category DTOToEntity(CategoryDTO category)
    {
        return new Category
        {
            Id = category.Id,
            Name = category.Name
        };
    }

    public static CategoryDTO EntityToDTO(Category category)
    {
        return new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name
        };
    }
}
