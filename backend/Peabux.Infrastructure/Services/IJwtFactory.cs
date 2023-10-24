using Peabux.Domain.Entities;
using System.Security.Claims;

namespace Peabux.Infrastructure.Services;

public interface IJwtFactory
{
    Task<List<Claim>> GenerateClaims(User user, string role);
    Task<string> GenerateToken(List<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
