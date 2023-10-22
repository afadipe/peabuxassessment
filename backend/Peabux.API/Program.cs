using Peabux.API.Extensions;
// Add services to the container.
var builder = WebApplication.CreateBuilder(args)
    .RegisterServices()
    .Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.SetupMiddleware()
    .Run();