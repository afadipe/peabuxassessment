namespace Peabux.Domain.Options;

public class JwtConfigOption
{
    public string? ValidIssuer { get; set; }
    public string? ValidAudience { get; set; }
    public int? Expires { get; set; }
    public string Secret { get; set; }
}