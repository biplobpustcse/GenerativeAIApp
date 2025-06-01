using Azure;
using Azure.AI.OpenAI;
using GenerativeAIApp.Core.DTOs;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;

namespace GenerativeAIApp.BLL.Services;

public class OpenAIService : IOpenAIService
{
    private readonly ChatClient _chatClient;
    private readonly List<ChatMessage> _messages;

    public OpenAIService(IConfiguration configuration)
    {
        var endpoint = configuration["AZURE_OPENAI_ENDPOINT"];
        var key = configuration["AZURE_OPENAI_KEY"];
        var deploymentName = configuration["AZURE_OPENAI_DEPLOYMENT_NAME"] ?? "gpt-35-turbo"; // Default or configured

        if (string.IsNullOrEmpty(endpoint))
        {
            Console.WriteLine("Please set the AZURE_OPENAI_ENDPOINT environment variable.");
            return;
        }

        if (string.IsNullOrEmpty(key))
        {
            Console.WriteLine("Please set the AZURE_OPENAI_KEY environment variable.");
            return;
        }

        AzureKeyCredential credential = new AzureKeyCredential(key);
        AzureOpenAIClient azureClient = new(new Uri(endpoint), credential);
        _chatClient = azureClient.GetChatClient(deploymentName);

        _messages = new List<ChatMessage>
        {
            new SystemChatMessage("You are an AI assistant that helps people find information."),
        };
    }

    public async Task<ChatResponseDTO> GetChatCompletionAsync(string userInput)
    {
        if (string.IsNullOrWhiteSpace(userInput))
        {
            return new ChatResponseDTO { BotResponse = "Please provide some input." };
        }

        _messages.Add(new UserChatMessage(userInput));

        var options = new ChatCompletionOptions
        {
            Temperature = (float)0.7,
            MaxOutputTokenCount = 800,
            TopP = (float)0.95,
            FrequencyPenalty = (float)0,
            PresencePenalty = (float)0,
        };

        try
        {
            ChatCompletion completion = await _chatClient.CompleteChatAsync(_messages, options);

            if (completion != null && completion.Content.FirstOrDefault()?.Text != null)
            {
                string botResponse = completion.Content.FirstOrDefault()?.Text;
                _messages.Add(new AssistantChatMessage(botResponse)); // Add assistant's response to maintain conversation history
                return new ChatResponseDTO { BotResponse = botResponse };
            }
            else
            {
                return new ChatResponseDTO { BotResponse = "No response received from the AI." };
            }
        }
        catch (Exception ex)
        {
            // Log the exception details here for debugging purposes
            Console.WriteLine($"An error occurred in OpenAI service: {ex.Message}");
            return new ChatResponseDTO { BotResponse = $"An error occurred: {ex.Message}" };
        }
    }
}
