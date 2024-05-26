using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure;

public static class QueryConfiguration
{
    public static IServiceCollection AddQueryServices(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services.AddDbContext<DatabaseContext>(
            options => options.UseNpgsql(configuration.GetConnectionString("SqlServer"))
        );

    public static IApplicationBuilder ConfigureQuery(this IApplicationBuilder app)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        if (environment is "Development")
        {
            var dbContext = app
                .ApplicationServices
                .CreateScope()
                .ServiceProvider
                .GetRequiredService<DatabaseContext>();

            dbContext.Database.Migrate();
        }

        return app;
    }
}