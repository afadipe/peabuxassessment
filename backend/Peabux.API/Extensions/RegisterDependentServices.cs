using System.Text.Json.Serialization;
using Peabux.API.Extensions.Shared;

namespace Peabux.API.Extensions;

public static class RegisterDependentServices
{
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .ConfigureOptions(builder.Configuration)
            .AddControllers(options =>
        {
            options.RespectBrowserAcceptHeader = true;
            options.ReturnHttpNotAcceptable = true;
            options.MaxIAsyncEnumerableBufferLimit = 8000;
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        })
        .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.ConfigureCors();
        builder.Services.ConfigureIISIntegration();
        builder.Services.ConfigureDBContext(builder.Configuration);
        builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.ConfigureVersioning();
        builder.Services.AddAuthentication();
        builder.Services.ConfigureIdentity();
        builder.Services.ConfigureJWTAuthentication();;
        builder.Services.ConfigureSwagger();

        return builder;
    }
}


