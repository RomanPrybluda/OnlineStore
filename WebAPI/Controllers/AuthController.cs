using Domain.Services.AuthService;
using Domain.Services.User;
using Domain.Services.User.DTO;
using Microsoft.AspNetCore.Mvc;


namespace WebAPI;
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly UserService _userService;
    private readonly TokenService _tokenService;

    public AuthController(AuthService authService, UserService userService, TokenService tokenService)
    {
        _authService = authService;
        _userService = userService;
        _tokenService = tokenService;
    }


    [HttpPost("Register")]
    public IActionResult Registration([FromBody] UserDTO user)
    {
        var succes = _authService.Register(user);

        if(succes == null)
        {
            return BadRequest(new { message = "User registration failed" });
        }
     
        return Ok(new
        {
            Token = succes
        });
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
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var token = await _userService.FargotPassword(request.Email);

        if (token == "User not found")
        {
            return NotFound(new { message = "User not found" });
        }

        
        return Ok(new { resetToken = token });
    }


    [HttpPost("Resset-password")]

    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var success = await _userService.ResetPassword(request.Token, request.NewPassword);

        if (!success)
        {
            return BadRequest(new { message = "Invalid token or user not found" });
        }

        return Ok(new { message = "Password successfully reset" });
    }



    public class ForgotPasswordRequest
    {
        public string Email { get; set; }
    }

    public class ResetPasswordRequest
    {
        public string Token { get; set; }

        public string NewPassword { get; set; }
    }
}
