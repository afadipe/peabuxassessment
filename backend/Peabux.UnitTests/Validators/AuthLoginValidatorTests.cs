using FluentValidation.TestHelper;
using Peabux.Domain.Dtos;

namespace Peabux.UnitTests.Validators;

public class AuthLoginValidatorTests
{
    private LoginRequestDtoValidator validator;
    public  AuthLoginValidatorTests()
    {
        validator = new LoginRequestDtoValidator();
    }

    [Fact]
    public void ValidPassword()
    {
        //Arrange
        var item = new LoginRequestDto
        {
            Password = "",
            UserName = "46464744"
        };
        //Act;
        var response = validator.TestValidate(item);
        //Assert
        response.ShouldHaveValidationErrorFor(x => x.Password).Only();
        response.ShouldNotHaveValidationErrorFor(x => x.UserName);
    }
}
