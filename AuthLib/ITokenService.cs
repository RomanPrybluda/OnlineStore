using System.Security.Claims;

namespace AuthLib;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken(Guid userId); 
    ClaimsPrincipal? GetPrincipalFromToken(string token);
}

