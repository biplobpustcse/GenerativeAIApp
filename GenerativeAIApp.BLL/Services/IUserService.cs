using GenerativeAIApp.Core.DTOs;
using GenerativeAIApp.Core.Entities;

namespace GenerativeAIApp.BLL.Services;

public interface IUserService
{
    Task<User?> ValidateUserAsync(string username, string password);
    Task<bool> RegisterUserAsync(UserRegisterDto dto);
}
