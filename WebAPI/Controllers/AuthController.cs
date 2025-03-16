using Domain.Services.AuthService;
using Domain.Services.AuthService.Login.DTO;
using Domain.Services.User.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI;
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("Register")]
    public IActionResult Registration([FromBody] UserDTO user)
    {
        var succes = _authService.Register(user);
     
        return Ok(new
        {
            Token = succes
        });
    }

    [HttpPost("Login")]
    public async Task<string> Login([FromBody] LoginDTO login)
    {
        var validUser = await _authService.Validate(login);
    
        return validUser;
    }


    [HttpPost("Logout")]
    public IActionResult Logout()
    {
        return Ok(new { message = "You have successfully logged out" });
    }
}