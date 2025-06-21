using System.Security.Claims;
using AuthLib;
using DAL;
using Domain.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Domain
{
    public class AppUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly CookieHelper _cookieHelper;
        private JwtOptions _jwtOptions;

      

        public AppUserService(UserManager<AppUser> userManager, 
            ITokenService tokenService, CookieHelper cookieHelper, IOptions<JwtOptions> options)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _cookieHelper = cookieHelper;
            _jwtOptions = options.Value;
        }

        public async Task<List<AppUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<AppUser?> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<AppUser?> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<bool> IsValidPassword(AppUser user, string password)
        {
            var result = await _userManager.CheckPasswordAsync(user, password);
            return result;
        }

        public async Task<IdentityResult> CreateTokensForUser(AppUser user, HttpResponse response)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? "")
            };
            try{
                var accessToken = _tokenService.GenerateAccessToken(claims);
                var refreshToken = _tokenService.GenerateRefreshToken(new Guid());
                _cookieHelper.AppendAuthCookies(response, accessToken, refreshToken);
                
                var result = await AddRefreshTokenToUser(user, refreshToken);
                
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            
        }

        public async Task<IdentityResult> AddRefreshTokenToUser(AppUser user, string refreshToken)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpirationDays);
            var result =  await _userManager.UpdateAsync(user);
            
            return result;
        }

        public async Task<IdentityResult> CreateUserAsync(AppUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
        
        public async Task<IdentityResult> RegisterUserAsync(RegisterDTO dto)
        {
            // Проверка существующего пользователя по Email или UserName
            if (await _userManager.FindByEmailAsync(dto.Email) is not null)
                throw new Exception("Користувач з таким email вже існує");

            if (await _userManager.FindByNameAsync(dto.UserName) is not null)
                throw new Exception("Користувач з таким username вже існує");

            var user = new AppUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                RefreshTokenExpiryTime = DateTime.MinValue,
                RefreshToken = null,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Age = dto.Age,
                
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new Exception(errors); 
            }
            
            return result;
        }

        public async Task<IdentityResult> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }

            return await _userManager.DeleteAsync(user);
        }
    }
}
