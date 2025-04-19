using Domain;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAPI
{
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
        public async Task<ActionResult> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult> GetUserByIdAsync([Required] Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteUserAsync([Required] Guid id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpGet("info/{id}")]
        public async Task<ActionResult> GetUserInfoAsync([Required] Guid id)
        {
            var userInfo = await _userService.GetUserInfoAsync(id);
            return Ok(userInfo);
        }

        [HttpPut("{id:Guid}")]
        public async Task<ActionResult> UpdateUserAsync(Guid id, [FromBody][Required] UpdateAppUserDTO request)
        {
            var result = await _userService.UserUpdateAsync(id, request);
            return Ok(result);
        }
    }
}
