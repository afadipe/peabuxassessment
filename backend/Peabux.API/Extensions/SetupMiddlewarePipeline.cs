using Microsoft.AspNetCore.HttpOverrides;

namespace Peabux.API.Extensions;

public static class SetupMiddlewarePipeline
{
    public static WebApplication SetupMiddleware(this WebApplication app)
    {
        // Configure the pipeline
        app.UseSwagger();
        app.UseSwaggerUI(s =>
        {
            s.SwaggerEndpoint("/swagger/v1/swagger.json", "Peabux Assessment API v1");
        });

        //using (var serviceScope = app.Services.CreateScope())
        //{
        //    using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<RepositoryContext>())
        //    {
        //        if (dbContext.Database.GetPendingMigrations().Any())
        //            dbContext.Database.Migrate();
        //    }
        //}


        //app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All
        });

        app.UseRateLimiter();
        app.UseCors("CorsPolicy");

        app.UseResponseCaching();

        app.UseAuthentication();
        app.UseAuthorization();

        //Add controller endpoint
        app.MapControllers();

        return app;
    }
}
