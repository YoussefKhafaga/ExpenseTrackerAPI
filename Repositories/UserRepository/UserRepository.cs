using System;
using System.Security.Claims;
using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Repositories.UserRepository;

public class UserRepository : IUserRepository
{
    private readonly ExpenseDBContext _context;
    public UserRepository(ExpenseDBContext context)

    {
        _context = context;
    }
    public async Task<User> CreateUserAsync(User user)
    {
        throw new NotImplementedException();
    }


}
