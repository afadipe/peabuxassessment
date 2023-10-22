using FluentValidation;

namespace Peabux.Domain.Dtos;

public record LoginRequestDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(x => x.UserName).NotNull().NotEmpty();
        RuleFor(x => x.Password).NotNull().NotEmpty();
    }
}
