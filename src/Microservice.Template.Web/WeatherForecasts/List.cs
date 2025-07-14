using Microservice.Template.UseCases;
using Microservice.Template.UseCases.WeatherForecast.List;

namespace Microservice.Template.Web.WeatherForecasts;

public class List(ISender _sender) : EndpointWithoutRequest<WeatherForecastListResponse>
{
  public override void Configure()
  {
    Get("/weatherforecast");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken cancellationToken)
  {
    Result<IEnumerable<WeatherForecastDTO>> result =
      await _sender.Send(new ListWeatherForecastQuery(null, null), cancellationToken);

    if (result.IsSuccess)
    {
      Response = new WeatherForecastListResponse()
      {
        Weathers = result.Value.Select(c => new WeatherForecastRecord(c.Date, c.TemperatureC, c.Summary, c.TemperatureF))
          .ToList()
      };
    }
  }
}
