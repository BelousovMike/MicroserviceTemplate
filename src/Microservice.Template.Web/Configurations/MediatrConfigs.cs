using Microservice.Template.Core.WeatherForecastAggregate;
using Microservice.Template.UseCases.WeatherForecast.List;

namespace Microservice.Template.Web.Configurations;

/// <summary>
/// Конфигурация MediatR.
/// </summary>
internal static class MediatrConfigs
{
    /// <summary>
    /// Метод-опсширение для конфигурации MediatR.
    /// </summary>
    /// <param name="services">Коллекция <see cref="IServiceCollection"/>.</param>
    /// <returns>Коллекция <see cref="IServiceCollection"/>с настроенным MediatR.</returns>
    public static IServiceCollection AddMediatrConfigs(this IServiceCollection services)
    {
        Assembly?[] mediatRAssemblies = new[]
        {
            Assembly.GetAssembly(typeof(WeatherForecast)), // из Core
            Assembly.GetAssembly(typeof(ListWeatherForecastQuery)), // из UseCases
        };

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

        return services;
    }
}
