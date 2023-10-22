using Microsoft.AspNetCore.Mvc;
using Peabux.Domain.Dtos;
using Peabux.Infrastructure.Services;

namespace Peabux.Presentation.Controllers;

[Route("api/token")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IAuthService _authService;
    public TokenController(IAuthService authService) => _authService = authService;

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto) =>  Ok(await _authService.RefreshToken(tokenDto));
}