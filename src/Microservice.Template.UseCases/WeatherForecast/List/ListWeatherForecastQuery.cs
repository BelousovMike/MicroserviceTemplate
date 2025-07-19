using Ardalis.SharedKernel;

namespace Microservice.Template.UseCases.WeatherForecast.List;

public record ListWeatherForecastQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<WeatherForecastDto>>>;
