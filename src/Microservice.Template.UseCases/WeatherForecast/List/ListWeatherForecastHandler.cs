using Ardalis.SharedKernel;

namespace Microservice.Template.UseCases.WeatherForecast.List;

/// <summary>
/// Обработчик получения списка <see cref="WeatherForecast"/>.
/// </summary>
/// <param name="service">Сервис получения списка <see cref="WeatherForecast"/>.</param>
public class ListWeatherForecastHandler(IListWeatherForecastQueryService service)
    : IQueryHandler<ListWeatherForecastQuery, Result<IEnumerable<WeatherForecastDto>>>
    {
    /// <summary>
    /// Обрабатывает запрос на получение списка прогнозов погоды.
    /// </summary>
    /// <param name="request">Запрос на получение списка прогнозов.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат выполнения запроса с коллекцией <see cref="WeatherForecastDto"/>.</returns>
    public async Task<Result<IEnumerable<WeatherForecastDto>>> Handle(
        ListWeatherForecastQuery request,
        CancellationToken cancellationToken)
        {
        IEnumerable<WeatherForecastDto> result = await service.GetAllAsync()
            .ConfigureAwait(false);
        return Result.Success(result);
    }
}
