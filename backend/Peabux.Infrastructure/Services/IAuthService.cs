using Microsoft.AspNetCore.Identity;
using Peabux.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peabux.Infrastructure.Services;

public interface IAuthService
{
    Task<IdentityResult> RegisterUser(RegistrationDto register);
    Task<bool> ValidateUser(LoginRequestDto login);
    Task<TokenDto> CreateToken(bool populateExp);
    Task<TokenDto> RefreshToken(TokenDto tokenDto);
}
