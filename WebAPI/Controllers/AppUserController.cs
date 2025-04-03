using DAL;
using Domain;
using Domain.Services.AppUser.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using SendGrid.Helpers.Errors.Model;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly AppUserService _userService;

        public AppUserController(AppUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("by-id/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] AppUser user)
        {
            var password = "User@123";
            var result = await _userService.CreateUserAsync(user, password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return NoContent();
        }

        [HttpGet("info/{userId}")]
        public async Task<IActionResult> GetUserInfo (string userId)
        {
            try
            {
                var userInfo = await _userService.GetUserInfoAsync(userId);  
                return Ok(userInfo);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);  
            }
            catch
            {
                return StatusCode(500, "Internal server error"); 
            }
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(typeof(UserUpdateResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser(
          [FromRoute] string userId,
          [FromBody] UserUpdateRequest request)
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
}
