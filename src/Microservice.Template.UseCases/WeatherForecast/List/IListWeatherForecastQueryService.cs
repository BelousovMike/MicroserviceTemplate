using Microservice.Template.Core;

namespace Microservice.Template.UseCases.WeatherForecast.List;

/// <summary>
/// Представляет собой службу, которая фактически извлекает необходимые данные
/// Обычно реализуемую в инфраструктуре.
/// </summary>
public interface IListWeatherForecastQueryService
{
    /// <summary>
    /// Получает список прогнозов погоды.
    /// </summary>
    /// <returns>Коллекция прогнозов погоды <see cref="WeatherForecastDto"/>.</returns>
    Task<IEnumerable<WeatherForecastDto>> GetAllAsync();
}
