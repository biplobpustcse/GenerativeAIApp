using GenerativeAIApp.BLL.Services;
using GenerativeAIApp.Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenerativeAIApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly TokenService _tokenService;

        #region Constractor

        public AuthController(UserService userService, TokenService tokenService)
        {
            this._userService = userService;
            this._tokenService = tokenService;
        }
        #endregion

        #region Methods

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            var success = await _userService.RegisterUserAsync(dto);
            return success ? Ok("User registered") : BadRequest("Username already exists");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            var user = await _userService.ValidateUserAsync(
                username: dto.Username,
                password: dto.Password
            );
            if (user is null)
                return BadRequest("Invalid username or password");

            var token = _tokenService.GenerateToken(user: user);
            return Ok(new { token });
        }

        [HttpGet("refresh-token")]
        public IActionResult RefreshToken(string token)
        {
            var newToken = _tokenService.GenerateRefreshToken(token);
            return newToken is null ? BadRequest("Invalid token") : Ok(new { token = newToken });
        }
        #endregion
    }
}
