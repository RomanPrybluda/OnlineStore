using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


namespace AuthLib;

public class TokenService:ITokenService
{
        private readonly JwtOptions _options;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IOptions<JwtOptions> options)
        {
            _options = options.Value;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            

           
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_options.AccessTokenExpirationMinutes),
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        
        public string GenerateRefreshToken(Guid userId)
        {
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var claims = new Claim[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_options.RefreshTokenExpirationDays),
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                SigningCredentials = creds
            };
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
        public ClaimsPrincipal? GetPrincipalFromToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _options.Issuer,
                ValidateAudience = true,
                ValidAudience = _options.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,
                ValidateLifetime = true,
            };

            var handler = new JwtSecurityTokenHandler();
            try
            {
                return handler.ValidateToken(token, tokenValidationParameters, out _);
            }
            catch
            {
                return null;
            }
        }
    
}

