using Domain.Services.AuthService;
using Domain.Services.User.DTO;
using Microsoft.AspNetCore.Mvc;
using Domain.Services.User;
using LoginDTO = Domain.Services.AuthService.DTO.LoginDTO;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Domain.Services.AuthService.DTO;

namespace WebAPI;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly OnlineStoreDbContext _dbContext;
    private readonly AuthService _authService;
    

    public AuthController(AuthService authService, OnlineStoreDbContext dbContext)
    {
        _dbContext = dbContext;
        _authService = authService;
    }

    [HttpPost("Register")]
    public IActionResult Registration([FromBody] RegisterDTO registerDto)
    {
        var token = _authService.Register(registerDto);

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
        if (ModelState.IsValid)
        {
            var result = await _authService.ForgotPasswordAsync(forgotPasswordDto.Email);
            if (result == "User not found")
            {
                return BadRequest("User not found.");
            }

            return Ok(new { Message = "Password reset token sent.", Token = result });
        }
        return BadRequest(ModelState);
    }

   
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDto)
    {
        if (ModelState.IsValid)
        {
            var result = await _authService.ResetPasswordAsync(resetPasswordDto.Email, resetPasswordDto.Token, resetPasswordDto.NewPassword);
            if (result)
            {
                return Ok("Password reset successfully.");
            }
            else
            {
                return BadRequest("Failed to reset password. Please check the token and try again.");
            }
        }
        return BadRequest(ModelState);
    }
}