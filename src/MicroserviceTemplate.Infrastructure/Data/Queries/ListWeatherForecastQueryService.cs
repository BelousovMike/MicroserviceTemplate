using MicroserviceTemplate.Core.WeatherForecastAggregate;
using MicroserviceTemplate.UseCases;
using MicroserviceTemplate.UseCases.WeatherForecast.List;

namespace MicroserviceTemplate.Infrastructure.Data.Queries;

public class ListWeatherForecastQueryService(AppDbContext db) : IListWeatherForecastQueryService
{
  private readonly AppDbContext _db = db;

  public async Task<IEnumerable<WeatherForecastDTO>> ListAsync()
  {
    var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
      Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
      TemperatureC = Random.Shared.Next(-20, 55),
      Summary = Summary.FromValue(Random.Shared.Next(Summary.List.Count - 1))
    })
      .Select(w => new WeatherForecastDTO(w.Date, w.TemperatureC, w.TemperatureF, w.Summary?.Name))
      .ToList();

    return await Task.FromResult(result);
  }
}
