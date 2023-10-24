using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Peabux.Domain.Entities;
using Peabux.Infrastructure.Services;
using Peabux.Presentation.Controllers;
using Peabux.UnitTests.Mocks;
using System.Net;

namespace Peabux.UnitTests.Controllers;

public class TokenControllerTest
{
    private readonly Mock<AuthService> _mockAuthService;
    private readonly Mock<UserManager<User>> _mockUserManager;

    public TokenControllerTest()
    {
        _mockUserManager = MockDIService.userManagerMockService();
        _mockAuthService = new Mock<AuthService>(Mock.Of<IMapper>(), _mockUserManager.Object,
            MockDIService.dBMockService().Object, MockDIService.JwtFactoryMockService().Object);
    }


    [Fact]
    public async void Post_WhenCalled_ReturnsOkResult()
    {
        var controller = new TokenController(_mockAuthService.Object);
        // Act
        IActionResult result = await controller.Refresh(ObjectBuilder.GetFakeTokenRequest());
        // assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        _mockAuthService.VerifyAll();
        _mockUserManager.Verify(x => x.FindByNameAsync(It.IsAny<string>()), Times.Once());
    }

    [Fact]
    public async void Post_WhenInvalidObjectPassed_ReturnsOkResult()
    {
        var controller = new TokenController(_mockAuthService.Object);
        // Act
        var model = ObjectBuilder.GetFakeTokenRequest();
        model.RefreshToken = string.Empty;
        var result = await controller.Refresh(model);
        var okResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.BadRequest, okResult.StatusCode);
        _mockAuthService.VerifyAll();
        _mockUserManager.Verify(x => x.FindByNameAsync(It.IsAny<string>()), Times.Once());
        _mockUserManager.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Never());
    }
}
