using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Peabux.Domain.Dtos;
using Peabux.Domain.Enums;

namespace Peabux.Presentation.Controllers;

[Route("api/lookup")]
[ApiController]
public class LookupController : ControllerBase
{

    /// <summary>
    /// Get All Title
    /// </summary>
    /// <returns>Array of titles</returns>
    [HttpGet("sitenames")]
    [ProducesResponseType(typeof(IEnumerable<NameValueDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTitleNames()
    {
        var titles = Enum.GetValues(typeof(EnumTitle))
                 .Cast<EnumTitle>()
                 .Select(x => new NameValueDto
                 {
                     Key = (int)x,
                     Value = x.ToString()
                 }).ToList();
        return Ok(titles);
    }
}
