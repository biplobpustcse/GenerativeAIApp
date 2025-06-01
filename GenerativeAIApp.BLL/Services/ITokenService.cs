using GenerativeAIApp.Core.Entities;

namespace GenerativeAIApp.BLL.Services;

public interface ITokenService
{
    string GenerateToken(User user);
    string GenerateRefreshToken(string token);
}
