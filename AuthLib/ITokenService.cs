using System.Security.Claims;

namespace AuthLib;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken(Guid userId); // обычно просто Guid.NewGuid()
    ClaimsPrincipal? GetPrincipalFromToken(string token);
}

