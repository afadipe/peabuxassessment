using Microsoft.AspNetCore.Http;
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
    [ProducesResponseType(typeof(IEnumerable<TokenDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto) =>  Ok(await _authService.RefreshToken(tokenDto));
}