using GenerativeAIApp.Core.DTOs;

namespace GenerativeAIApp.BLL.Services;

public interface IOpenAIService
{
    Task<ChatResponseDTO> GetChatCompletionAsync(string userInput);
}
