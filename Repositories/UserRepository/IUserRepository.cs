using System;
using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Repositories.UserRepository;

public interface IUserRepository
{
    //Task<IEnumerable<User>> GetAllUsersAsync();
    //Task<string> GetCurrentUserAsync();
    Task<User> CreateUserAsync(User user);
    //Task<User> UpdateUserAsync(User user);
    //Task<bool> DeleteUserAsync(int id);
}
