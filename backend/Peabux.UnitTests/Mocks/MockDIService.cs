using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Peabux.Domain.Entities;
using Peabux.Infrastructure;
using Peabux.Infrastructure.Services;
using System.Security.Claims;

namespace Peabux.UnitTests.Mocks;

public static class MockDIService
{

    public static Mock<IJwtFactory> JwtFactoryMockService()
    {
        var mockJwtFactory = new Mock<IJwtFactory>();
        mockJwtFactory.Setup(x => x.GenerateClaims(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(ObjectBuilder.GetFakeClaim());
        mockJwtFactory.Setup(x => x.GenerateToken(It.IsAny<List<Claim>>())).ReturnsAsync("TestToken");
        mockJwtFactory.Setup(x => x.GetPrincipalFromExpiredToken(It.IsAny<string>())).Returns(ObjectBuilder.GetFakeClaimsPrincipal);
        return mockJwtFactory;

    }

    public static Mock<UserManager<User>> userManagerMockService()
    {
        var mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
        mockUserManager.Setup(x => x.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);

        mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>())).Returns(Task.FromResult(IdentityResult.Success));
        mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), "Teacher")).Returns(Task.FromResult(IdentityResult.Success));
        mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(ObjectBuilder.GetFakeUser()));
        return mockUserManager;
    }

    public static Mock<IDbContextFactory<AppDBContext>> dBMockService()=> new Mock<IDbContextFactory<AppDBContext>>();



}
