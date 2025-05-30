using System;
using ExpenseTrackerAPI.Repositories.CategoryRepository;
using ExpenseTrackerAPI.Repositories.ExpenseRepository;
using ExpenseTrackerAPI.Repositories.UserRepository;

namespace ExpenseTrackerAPI.Data.UnitOfWork;

public interface IUnitOfWork
{
        Task<int> SaveChangesAsync();
    void Dispose();
    public ICategoryRepository Categories { get; }
    public IUserRepository Users { get; }
    public IExpenseRepository Expenses { get; }
}
