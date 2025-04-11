using DAL;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Errors.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("appusers")]
[Authorize(Roles ="Admin")]
public class AppUserController : ControllerBase
{
    private readonly AppUserService _userService;
    private readonly UserManager<AppUser> _userManager;

    public AppUserController(AppUserService userService, UserManager<AppUser> userManager)
    {
        _userManager = userManager;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("by-id/{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] RegisterDTO registerDto)
    {
        var appUser = RegisterDTO.FromRegisterDTO(registerDto);
        var password = registerDto.Password;

        var result = await _userService.CreateUserAsync(appUser, password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return CreatedAtAction(nameof(GetUserById), new { id = appUser.Id }, appUser);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        return NoContent();
    }

    [HttpGet("info/{userId}")]
    public async Task<IActionResult> GetUserByIdAsync(Guid userId)
    {
        var userById = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId.ToString());
        if (userById == null)
        {
            return NotFound(new { message = "User not found" });
        }

        var userDTO = UserInfoDTO.FromAppUser(userById);
        return Ok(userDTO);
    }

    [HttpPut("{userId}")]
    [ProducesResponseType(typeof(UserUpdateResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUser([FromRoute] string userId, [FromBody] UserUpdateRequest request)
    {
        try
        {
            var result = await _userService.UserUpdateAsync(userId, request);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
