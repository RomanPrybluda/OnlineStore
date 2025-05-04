using Domain;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI
{

    [ApiController]
    [Route("auth")]

    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var token = await _authService.Register(registerDto);
            return Ok(new { Token = token });
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var token = await _authService.Validate(loginDto);
            return Ok(new { Token = token });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { Message = "You have successfully logged out" });
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPasswordDto)
        {
            var token = await _authService.ForgotPasswordAsync(forgotPasswordDto.Email);
            return Ok(new { Message = "Password reset token sent", Token = token });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDto)
        {
            await _authService.ResetPasswordAsync(resetPasswordDto.Email, resetPasswordDto.Token, resetPasswordDto.NewPassword);
            return Ok("Password reset successfully.");
        }
    }
}