using Microsoft.AspNetCore.Mvc;
using Peabux.Domain.Dtos;
using Peabux.Domain.Entities;
using Peabux.Infrastructure.Services;

namespace Peabux.Presentation.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService  _authService;
    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("login")]
    public async Task<IActionResult> Authenticate([FromBody] LoginRequestDto login)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _authService.ValidateUser(login))
            return Unauthorized();

        var tokenDto = await _authService.CreateToken(populateExp: true);

        return Ok(tokenDto);
    }
}
