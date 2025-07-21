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
    /// Конфигурирование.
    /// </summary>
    public override void Configure()
    {
        Get("/weatherforecast");
        AllowAnonymous();
    }

    /// <summary>
    /// Выполняет обработку HTTP GET запроса для получения списка прогнозов.
    /// </summary>
    /// <param name="ct">Токен для отмены асинхронной операции.</param>
    /// <returns>
    /// <see cref="Task"/>, представляющий асинхронную операцию обработки запроса.
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
                Weathers = result.Value.Select(c => new WeatherForecastRecord(
                        c.Date,
                        c.TemperatureC,
                        c.Summary,
                        c.TemperatureF))
                    .ToList(),
            };
        }
    }
}
