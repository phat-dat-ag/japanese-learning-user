using JapaneseLearning.User.Infrastructure.Configuration;
using JapaneseLearning.User.Infrastructure.Database;
using JapaneseLearning.User.Infrastructure.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JapaneseLearning.User.Application.Abstractions.Persistence;
using JapaneseLearning.User.Application.Abstractions.Security;
using JapaneseLearning.User.Infrastructure.Persistence.Repositories;
using JapaneseLearning.User.Infrastructure.Security;

namespace JapaneseLearning.User.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<DatabaseOptions>()
            .Bind(configuration.GetSection(DatabaseOptions.SectionName))
            .Validate(
                options => !string.IsNullOrWhiteSpace(
                    options.ConnectionString),
                "Database connection string is missing or empty.")
            .ValidateOnStart();

        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();

        services.AddHealthChecks()
            .AddCheck<SqlServerHealthCheck>(
                "sql-server");

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}