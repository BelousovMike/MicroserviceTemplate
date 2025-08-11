using Ardalis.SharedKernel;

namespace Microservice.Template.UseCases.WeatherForecast.List;

/// <summary>
/// Запрос списка прогнозов погоды.
/// </summary>
/// <param name="Skip">Количество для пропуска.</param>
/// <param name="Take">Количество для получения.</param>
public record ListWeatherForecastQuery(int? Skip, int? Take) : IQuery<Result<IEnumerable<WeatherForecastDto>>>;
