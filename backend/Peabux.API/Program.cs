using Peabux.API.Extensions;
// Add services to the container.
var builder = WebApplication.CreateBuilder(args)
    .RegisterServices()
    .Build();

builder.SetupMiddleware()
    .Run();