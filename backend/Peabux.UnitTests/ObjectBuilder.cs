using Peabux.Domain.Dtos;
using Peabux.Domain.Entities;
using System.Security.Claims;

namespace Peabux.UnitTests;

public static class ObjectBuilder
{
    public static LoginRequestDto GetFakeLoginRequest() => new() { UserName = "76876387464", Password = "Password" };
    public static TokenDto GetFakeTokenRequest() => new() { AccessToken = "76876387464", RefreshToken = "78345" };
    public static User GetFakeUser() => new() { RefreshToken= "78345", RefreshTokenExpiryTime = DateTime.Now.AddMonths(+2), UserName = "JFakeNationalId", DateOfBirth = new DateTime(), NationalIdNumber = "JFakeNationalId" };

    public static List<Claim> GetFakeClaim()
    {
        return new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName, "JO Fake"),
                new Claim(ClaimTypes.Name, "JFakeNationalId"),
                new Claim(ClaimTypes.NameIdentifier, "42444-442424-242424-5454545"),
                new Claim(ClaimTypes.Role, "Teacher"),
            };
    }

    public static ClaimsPrincipal GetFakeClaimsPrincipal() => new (new ClaimsIdentity(GetFakeClaim(), "TestAuthType"));


}
