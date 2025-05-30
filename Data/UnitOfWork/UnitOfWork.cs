using System;
using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Data.UnitOfWork;
using ExpenseTrackerAPI.Repositories.CategoryRepository;
using ExpenseTrackerAPI.Repositories.ExpenseRepository;
using ExpenseTrackerAPI.Repositories.UserRepository;

namespace ExpenseTrackerAPI.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ExpenseDBContext _context;
    private ICategoryRepository _categoryRepository;
    private IUserRepository _userRepository;
    private IExpenseRepository _expenseRepository;
    public UnitOfWork(ExpenseDBContext context)
    {
        _context = context;
    }
    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
    public void Dispose()
    {
        _context.Dispose();
    }
    public ICategoryRepository Categories =>
    _categoryRepository ??= new CategoryRepository(_context);

    public IUserRepository Users =>
        _userRepository ??= new UserRepository(_context);

    public IExpenseRepository Expenses =>
        _expenseRepository ??= new ExpenseRepository(_context);

}
