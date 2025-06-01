using GenerativeAIApp.BLL.Services;
using GenerativeAIApp.Core.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GenerativeAIApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OpenAIController : ControllerBase
{
    private readonly IOpenAIService _openAIService;

    public OpenAIController(IOpenAIService openAIService)
    {
        _openAIService = openAIService;
    }

    [HttpPost("chat")]
    public async Task<ActionResult<ChatResponseDTO>> Chat([FromBody] ChatRequestDTO request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.UserInput))
        {
            return BadRequest("User input cannot be empty.");
        }

        var response = await _openAIService.GetChatCompletionAsync(request.UserInput);
        return Ok(response);
    }
}
