using Microsoft.AspNetCore.Identity;
using Peabux.Domain.Dtos;

namespace Peabux.Infrastructure.Services;

public interface IAuthService
{
    Task<IdentityResult> RegisterUser(RegistrationDto register);
    Task<bool> ValidateUser(LoginRequestDto login);
    Task<TokenDto> CreateToken(bool populateExp);
    Task<TokenDto> RefreshToken(TokenDto tokenDto);
}
