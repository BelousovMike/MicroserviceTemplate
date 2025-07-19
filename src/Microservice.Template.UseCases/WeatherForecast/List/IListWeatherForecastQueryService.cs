using Microservice.Template.Core;

namespace Microservice.Template.UseCases.WeatherForecast.List;

/// <summary>
/// Представляет собой службу, которая фактически извлекает необходимые данные
/// Обычно реализуемую в инфраструктуре.
/// </summary>
public interface IListWeatherForecastQueryService
{
    /// <summary>
    /// Метод получения информации о погоде.
    /// </summary>
    /// <returns>Возвращает коллекцию <see cref="WeatherForecastDto"/> или пустую коллекцию.</returns>
    Task<IEnumerable<WeatherForecastDto>> ListAsync();
}
