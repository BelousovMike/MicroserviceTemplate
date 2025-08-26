using Microservice.Template.Core.WeatherForecastAggregate;
using Microservice.Template.UseCases;
using Microservice.Template.UseCases.WeatherForecast.List;

namespace Microservice.Template.Infrastructure.Data.Queries;

/// <summary>
/// Сервис получения данных о погоде.
/// </summary>
/// <param name="db">Контекст БД.</param>
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Style",
    "CA1823",
    Justification = "Демо код. Добавлено для примера использования с БД")]
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Style",
    "SA1309",
    Justification = "Демо код. Добавлено для примера использования с БД")]
public class ListWeatherForecastQueryService(AppDbContext db) : IListWeatherForecastQueryService
{
    private readonly AppDbContext _db = db;

    /// <summary>
    /// Получает список прогнозов погоды.
    /// </summary>
    /// <returns>Коллекция прогнозов погоды <see cref="WeatherForecastDto"/>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Security",
        "CA5394:Do not use insecure randomness",
        Justification = "Пример. Не используется в Production")]
    public async Task<IEnumerable<WeatherForecastDto>> GetAllAsync()
    {
        var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summary.FromValue(Random.Shared.Next(Summary.List.Count - 1)),
        })
            .Select(w => new WeatherForecastDto(w.Date, w.TemperatureC, w.TemperatureF, w.Summary?.Name))
            .ToList();

        return await Task.FromResult(result)
            .ConfigureAwait(true);
    }
}
