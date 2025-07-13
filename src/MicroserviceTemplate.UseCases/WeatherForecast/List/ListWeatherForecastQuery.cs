using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MicroserviceTemplate.UseCases.WeatherForecast.List;

public record ListWeatherForecastQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<WeatherForecastDTO>>>;
