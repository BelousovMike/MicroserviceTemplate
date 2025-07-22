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
    /// Обработка комманды.
    /// </summary>
    /// <param name="request">Запрос.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Коллекция <see cref="WeatherForecastDto"/>.</returns>
    public async Task<Result<IEnumerable<WeatherForecastDto>>> Handle(
        ListWeatherForecastQuery request,
        CancellationToken cancellationToken)
        {
        IEnumerable<WeatherForecastDto> result = await service.ListAsync()
            .ConfigureAwait(false);
        return Result.Success(result);
    }
}
