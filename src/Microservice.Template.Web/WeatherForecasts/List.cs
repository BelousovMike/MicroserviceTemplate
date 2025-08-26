using Microservice.Template.UseCases;
using Microservice.Template.UseCases.WeatherForecast.List;

namespace Microservice.Template.Web.WeatherForecasts;

/// <summary>
/// GET эндпоинт для получения списка прогнозов погоды.
/// </summary>
/// <param name="sender">MediatR sender для обработки <see cref="ListWeatherForecastQuery"/>.</param>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Performance",
    "CA1812:Avoid uninstantiated internal classes",
    Justification = "FastEndpoints создает экземпляры объектов автоматически.")]
internal sealed class List(ISender sender) : EndpointWithoutRequest<WeatherForecastListResponse>
{
    /// <summary>
    /// Настраивает эндпоинт для получения списка прогнозов погоды.
    /// </summary>
    public override void Configure()
    {
        Get("/weatherforecast");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Получить прогноз погоды";
            s.Description = "Возвращает список прогнозов на 5 дней.";
            s.Responses[200] = "Список прогнозов погоды успешно получен.";
            s.Responses[400] = "Некорректный запрос.";
            s.Responses[500] = "Внутренняя ошибка сервера.";
        });
    }

    /// <summary>
    /// Обрабатывает HTTP GET запрос для получения списка прогнозов погоды.
    /// </summary>
    /// <param name="ct">Токен для отмены асинхронной операции.</param>
    /// <returns>
    /// Задача, представляющая асинхронную операцию обработки запроса.
    /// </returns>
    public override async Task HandleAsync(CancellationToken ct)
    {
        Result<IEnumerable<WeatherForecastDto>> result =
            await sender.Send(new ListWeatherForecastQuery(null, null), ct)
                .ConfigureAwait(false);

        if (result.IsSuccess)
        {
            Response = new WeatherForecastListResponse()
            {
                Forecasts =
                [
                    .. result.Value.Select(c => new WeatherForecastRecord(
                        c.Date,
                        c.TemperatureC,
                        c.Summary,
                        c.TemperatureF))
                ],
            };
        }
    }
}
