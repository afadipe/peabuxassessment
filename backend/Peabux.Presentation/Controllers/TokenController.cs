﻿using Microsoft.AspNetCore.Http;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto) 
    {
        var token =await _authService.RefreshToken(tokenDto);
        return (token == null) ? BadRequest("Invalid Request") : Ok(token);
    } 
}