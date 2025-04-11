using Domain;
using Domain.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI;

[ApiController]
[Route("/auth")]
public class AuthController : ControllerBase
{

    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Registration([FromBody] RegisterDTO registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var token = await _authService.Register(registerDto);

        if (token == null)
        {
            return BadRequest(new { message = "User registration failed" });
        }

        return Ok(new { Token = token });
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO login)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var token = await _authService.Validate(login);

        if (token == null)
        {
            return Unauthorized("Invalid email or password");
        }

        return Ok(new { Token = token });
    }

    [HttpPost("Logout")]
    public IActionResult Logout()
    {

        return Ok(new { message = "You have successfully logged out" });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPasswordDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.ForgotPasswordAsync(forgotPasswordDto.Email);
        if (result == "User not found")
        {
            return BadRequest("User not found.");
        }

        return Ok(new { Message = "Password reset token sent.", Token = result });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.ResetPasswordAsync(resetPasswordDto);
        if (result)
        {
            return Ok("Password reset successfully.");
        }

        return BadRequest("Failed to reset password. Please check the token and try again.");
    }
}
