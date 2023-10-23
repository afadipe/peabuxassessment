using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Peabux.Domain.Dtos;
using Peabux.Infrastructure.Services;

namespace Peabux.Presentation.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService  _authService;
    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Authenticate([FromBody] LoginRequestDto login)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _authService.ValidateUser(login))
            return Unauthorized();

        var tokenDto = await _authService.CreateToken(populateExp: true);

        return Ok(tokenDto);
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegisterUser([FromBody] RegistrationDto register)
    {
        var result = await _authService.RegisterUser(register);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }

        return StatusCode(201);
    }
}
