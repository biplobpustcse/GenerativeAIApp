using GenerativeAIApp.Core.Entities;

namespace GenerativeAIApp.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task AddUserAsync(User user);
}
