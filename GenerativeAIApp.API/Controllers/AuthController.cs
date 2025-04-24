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
        public async Task<ActionResult> Login(UserLoginDto userLoginDto)
        {
            var user = await _userService.ValidateUserAsync(
                username: userLoginDto.Username,
                password: userLoginDto.Password
            );
            if (user == null)
                return Unauthorized("Invalid credentials");

            var token = _tokenService.GenerateToken(user: user);
            return Ok(new { token });
        }

        #endregion
    }
}
