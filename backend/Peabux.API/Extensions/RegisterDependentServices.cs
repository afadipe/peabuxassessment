using System.Text.Json.Serialization;
using Peabux.API.Extensions.Shared;
using Peabux.Domain.Dtos;
using Peabux.Infrastructure.Services;
using Peabux.Presentation.ActionFilters;
using FluentValidation;
using FluentValidation.AspNetCore;

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
        .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly)
        .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegistrationValidator>());
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.ConfigureCors();
        builder.Services.ConfigureIISIntegration();
        builder.Services.ConfigureDBContext(builder.Configuration);
        builder.Services.AddAutoMapper(typeof(Infrastructure.AutoMapperProfile.MappingProfile));
       // builder.Services.AddScoped<ValidationFilterAttribute>();
        builder.Services.ConfigureVersioning();
        builder.Services.AddAuthentication();
        builder.Services.ConfigureIdentity();
        builder.Services.ConfigureJWTAuthentication();;
        builder.Services.ConfigureSwagger();
        builder.Services.AddScoped<IAuthService, AuthService>();
        

        return builder;
    }
}


