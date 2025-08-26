using Microservice.Template.Core.WeatherForecastAggregate;
using Microservice.Template.UseCases.WeatherForecast.List;

namespace Microservice.Template.Web.Configurations;

/// <summary>
/// Конфигурация MediatR.
/// </summary>
internal static class MediatrConfigs
{
    /// <summary>
    /// Добавляет конфигурацию MediatR в DI-контейнер.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Коллекция сервисов с настроенным MediatR.</returns>
    public static IServiceCollection AddMediatrConfigs(this IServiceCollection services)
    {
        Assembly?[] mediatRAssemblies = new[]
        {
            Assembly.GetAssembly(typeof(WeatherForecast)), // из Core
            Assembly.GetAssembly(typeof(ListWeatherForecastQuery)), // из UseCases
        };

        return services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
    }
}
