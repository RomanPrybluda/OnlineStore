using Domain;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("users")]

public class AppUserController : ControllerBase
{
    private readonly AppUserService _userService;

    public AppUserController(AppUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserByIdAsync(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return Ok(user);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserAsync(Guid id)
    {
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserAsync([FromRoute] string id, [FromBody] UserUpdateDTO request)
    {
        var user = await _userService.UpdateUserAsync(id, request);
        return Ok(user);
    }
}
