using JapaneseLearning.User.Infrastructure.Database;
using JapaneseLearning.User.Infrastructure.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JapaneseLearning.User.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'DefaultConnection' is missing or empty.");
        }

        services.AddSingleton<ISqlConnectionFactory>(
            new SqlConnectionFactory(connectionString));

        services.AddHealthChecks()
            .AddCheck<SqlServerHealthCheck>(
                "sql-server");

        return services;
    }
}