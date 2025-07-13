using MicroserviceTemplate.Infrastructure;

namespace MicroserviceTemplate.Web.Configurations;

public static class ServiceConfigs
{
  public static IServiceCollection AddServiceConfigs(this IServiceCollection services, Microsoft.Extensions.Logging.ILogger logger, WebApplicationBuilder builder)
  {
    services.AddInfrastructureServices(builder.Configuration, logger)
            .AddMediatrConfigs();


    if (builder.Environment.IsDevelopment())
    {
      // Можно добавлять сервисы для локального тестирования
      //services.AddScoped<IAnyService, AnyService>();

    }
    else
    {
      // Можно добавлять сервисы которые нужны не только для Development.
      //services.AddScoped<IAnyService, AnyService>();
    }

    logger.LogInformation("{Project} services registered", "Mediatr and AnyService");

    return services;
  }


}
