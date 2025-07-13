namespace MicroserviceTemplate.UseCases.WeatherForecast.List;
/// <summary>
/// Представляет собой службу, которая фактически извлекает необходимые данные
/// Обычно реализуемую в инфраструктуре
/// </summary>
public interface IListWeatherForecastQueryService
{
  Task<IEnumerable<WeatherForecastDTO>> ListAsync();
}
