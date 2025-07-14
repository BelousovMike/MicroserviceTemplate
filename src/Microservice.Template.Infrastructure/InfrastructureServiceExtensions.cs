using Microservice.Template.Infrastructure.Data;
using Microservice.Template.Infrastructure.Data.Queries;
using Microservice.Template.UseCases.WeatherForecast.List;

namespace Microservice.Template.Infrastructure;
public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger)
  {
    string? connectionString = config.GetConnectionString("SqlConnection");
    Guard.Against.Null(connectionString);
    services.AddDbContext<AppDbContext>(options =>
     options.UseSqlServer(connectionString));

    // Интерфейсы и сервисы из Core тоже нужно подключать здесь.
    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>))
           .AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>))
           .AddScoped<IListWeatherForecastQueryService, ListWeatherForecastQueryService>();


    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }
}
