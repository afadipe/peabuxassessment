
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Peabux.Domain.Entities;
using Peabux.Domain.Options;
using Asp.Versioning;
using Peabux.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Peabux.API.Extensions.Shared;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureOptions(this IServiceCollection @this, IConfiguration configuration) =>
        @this
        .Configure<JwtConfigOption>(configuration.GetSection(nameof(JwtConfigOption)))
        .AddSingleton(x => x.GetRequiredService<IOptions<JwtConfigOption>>().Value);


    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("X-Pagination"));
        });

    public static void ConfigureIISIntegration(this IServiceCollection services) =>
        services.Configure<IISOptions>(options =>
        {
        });


    public static void ConfigureDBContext(this IServiceCollection @this, IConfiguration configuration) =>
        @this.AddDbContextFactory<AppDBContext>(opts => opts.UseNpgsql(configuration.GetConnectionString("DBConnection")));


    public static void ConfigureVersioning(this IServiceCollection services)=> 
    services.AddApiVersioning(opt =>
    {
        opt.ReportApiVersions = true;
        opt.AssumeDefaultVersionWhenUnspecified = true;
        opt.DefaultApiVersion = new ApiVersion(1, 0);
        opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
    });

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentity<User, IdentityRole>(o =>
        {
            o.Password.RequireDigit = true;
            o.Password.RequireLowercase = false;
            o.Password.RequireUppercase = false;
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequiredLength = 10;
            o.User.RequireUniqueEmail = false;
            o.Password.RequiredUniqueChars = 0;
            // Signin settings
            o.SignIn.RequireConfirmedEmail = false;
            o.SignIn.RequireConfirmedPhoneNumber = false;
        })
        .AddEntityFrameworkStores<AppDBContext>()
        .AddDefaultTokenProviders();
    }

    public static void ConfigureJWTAuthentication(this IServiceCollection services)
    {
        var jwtConfig = services.BuildServiceProvider().GetRequiredService<JwtConfigOption>();

         services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtConfig.ValidIssuer,
                ValidAudience = jwtConfig.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret)),
                ClockSkew = TimeSpan.Zero
            };
        });
    }

    public static void ConfigureSwagger(this IServiceCollection services)=>
         services.AddSwaggerGen(s =>
         {
             s.SwaggerDoc("v1", new OpenApiInfo
             {
                 Title = "Peabux API",
                 Version = "v1",
                 Description = "Peabux API  Assessment",
                 TermsOfService = new Uri("https://example.com/terms"),
                 Contact = new OpenApiContact
                 {
                     Name = "John Doe",
                     Email = "John.Doe@gmail.com",
                     Url = new Uri("https://twitter.com/johndoe"),
                 },
                 License = new OpenApiLicense
                 {
                     Name = "Peabux API LICX",
                     Url = new Uri("https://example.com/license"),
                 }
             });

             //var xmlFile = $"{typeof(Presentation.AssemblyReference).Assembly.GetName().Name}.xml";
             //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
             //s.IncludeXmlComments(xmlPath);

             s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
             {
                 In = ParameterLocation.Header,
                 Description = "Place to add JWT with Bearer",
                 Name = "Authorization",
                 Type = SecuritySchemeType.ApiKey,
                 Scheme = "Bearer"
             });

             s.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Name = "Bearer",
                    },
                    new List<string>()
                }
            });
         });
}
