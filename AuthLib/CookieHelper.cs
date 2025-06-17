using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace AuthLib;

public class CookieHelper
{
    private JwtOptions _options;

    CookieHelper(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }
    
    public void AppendAuthCookies(HttpResponse response, string accessToken, string refreshToken)
    {
        response.Cookies.Append("access_token", accessToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(_options.AccessTokenExpirationMinutes)
        });

        response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(_options.RefreshTokenExpirationDays)
        });
    }

    public void ClearAuthCookies(HttpResponse response)
    {
        response.Cookies.Delete("access_token");
        response.Cookies.Delete("refresh_token");
    }
}
