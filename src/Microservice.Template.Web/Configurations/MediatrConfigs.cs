using System.Reflection;
using Ardalis.SharedKernel;
using Microservice.Template.Core.WeatherForecastAggregate;
using Microservice.Template.UseCases.WeatherForecast.List;

namespace Microservice.Template.Web.Configurations;

public static class MediatrConfigs
{
  public static IServiceCollection AddMediatrConfigs(this IServiceCollection services)
  {
    var mediatRAssemblies = new[]
      {
        Assembly.GetAssembly(typeof(WeatherForecast)), // из Core
        Assembly.GetAssembly(typeof(ListWeatherForecastQuery)) // из UseCases
      };

    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();

    return services;
  }
}
