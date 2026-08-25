using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace JapaneseLearning.User.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);

            cfg.AddOpenBehavior(
                typeof(Common.Behaviors.ValidationBehavior<,>));

            cfg.AddOpenBehavior(
                typeof(Common.Behaviors.LoggingBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}