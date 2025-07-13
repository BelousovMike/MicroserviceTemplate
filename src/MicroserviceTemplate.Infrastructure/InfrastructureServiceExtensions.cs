using MicroserviceTemplate.Infrastructure.Data;
using MicroserviceTemplate.Infrastructure.Data.Queries;
using MicroserviceTemplate.UseCases.WeatherForecast.List;

namespace MicroserviceTemplate.Infrastructure;
public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger)
  {
    string? connectionString = config.GetConnectionString("SqliteConnection");
    Guard.Against.Null(connectionString);
    services.AddDbContext<AppDbContext>(options =>
     options.UseSqlite(connectionString));

    // Интерфейсы и сервисы из Core тоже нужно подключать здесь.
    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
           .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
           .AddScoped<IListWeatherForecastQueryService, ListWeatherForecastQueryService>();


    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }
}
