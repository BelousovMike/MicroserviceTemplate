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
    /// Добавляет инфраструктурные сервисы в DI-контейнер.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="config">Конфигурация приложения.</param>
    /// <returns>Коллекция сервисов с добавленными инфраструктурными сервисами.</returns>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        ConfigurationManager config) =>
            services.AddDbContext(config)
                .AddDataAccessServices();

    /// <summary>
    /// Добавляет контекст базы данных в коллекцию сервисов.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="config">Конфигурация приложения.</param>
    /// <returns>Коллекция сервисов с добавленным контекстом базы данных.</returns>
    private static IServiceCollection AddDbContext(this IServiceCollection services, ConfigurationManager config)
    {
        var connectionString = config.GetConnectionString("DefaultConnection");
        Guard.Against.Null(connectionString);
        services.AddApplicationDbContext(connectionString);
        return services;
    }

    /// <summary>
    /// Добавляет сервисы доступа к данным в коллекцию сервисов.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Коллекция сервисов с добавленными сервисами доступа к данным.</returns>
    private static IServiceCollection AddDataAccessServices(this IServiceCollection services) =>
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
            .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
            .AddScoped<IListWeatherForecastQueryService, ListWeatherForecastQueryService>();
}
