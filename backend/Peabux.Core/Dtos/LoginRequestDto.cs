using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peabux.Domain.Dtos;

public record LoginRequestDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
}
