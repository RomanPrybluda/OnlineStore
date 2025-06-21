using System.Security.Claims;
using AuthLib;
using Domain;
using Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly CookieHelper _cookieHelper;
    private readonly AppUserService _appUserService;

    public AuthController(ITokenService tokenService,CookieHelper cookieHelper,AppUserService appUserService)
    {
        _tokenService = tokenService;
        _cookieHelper = cookieHelper;
        _appUserService = appUserService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        var result = await _appUserService.RegisterUserAsync(dto);
        return Ok(result.ToString());
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        // validate user (from db, etc.)
        
        var user = await _appUserService.GetUserByEmailAsync(dto.Email); 
        if (user == null)
                return Unauthorized("Невірна пошта або пароль");
        
        
        var isValid = await _appUserService.IsValidPassword(user, dto.Password);
        if (!isValid)
            return Unauthorized("Невірна пошта або пароль");
        
        var result = await _appUserService.CreateTokensForUser(user, Response);
        
        if (!result.Succeeded)
            return Unauthorized();
        
        return Ok(result.ToString());
    }

    [HttpPost("refresh")]
    public IActionResult Refresh()
    {
      
        var refreshToken = Request.Cookies["refresh_token"] ?? "";

        var principal = _tokenService.GetPrincipalFromToken(refreshToken);
        if (principal == null) return Unauthorized();

     
        
        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
       
        
        _cookieHelper.AppendAuthCookies(Response, newAccessToken, refreshToken);

        return Ok();
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var accessToken = Request.Cookies["access_token"];
       
        var principal = _tokenService.GetPrincipalFromToken(accessToken ?? "");
        if (principal != null)
        {
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier)!;
            //db del refresh
        }

        _cookieHelper.ClearAuthCookies(Response);
        return Ok();
    }
}
