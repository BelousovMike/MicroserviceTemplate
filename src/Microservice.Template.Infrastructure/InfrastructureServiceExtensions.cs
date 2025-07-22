using Microservice.Template.Infrastructure.Data;
using Microservice.Template.Infrastructure.Data.Queries;
using Microservice.Template.UseCases.WeatherForecast.List;

namespace Microservice.Template.Infrastructure;

/// <summary>
/// Расширение для добавления инфраструктурных сервисов.
/// </summary>
public static class InfrastructureServiceExtensions
{
    /// <summary>
    /// Добавление инфраструктурных сервисов в DI.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="config">Конфигурация.</param>
    /// <returns><see cref="IServiceCollection"/> с добавленными инфраструктурными сервисами.</returns>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        ConfigurationManager config)
    {
        var connectionString = config.GetConnectionString("DefaultConnection");
        Guard.Against.Null(connectionString);
        services.AddApplicationDbContext(connectionString);

        // Интерфейсы и сервисы из Core тоже нужно подключать здесь.
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
            .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
            .AddScoped<IListWeatherForecastQueryService, ListWeatherForecastQueryService>();

        return services;
    }
}
