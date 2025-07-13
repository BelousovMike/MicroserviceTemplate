using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MicroserviceTemplate.UseCases.WeatherForecast.List;

public class ListWeatherForecastHandler(IListWeatherForecastQueryService _service)
  : IQueryHandler<ListWeatherForecastQuery, Result<IEnumerable<WeatherForecastDTO>>>
{
  public async Task<Result<IEnumerable<WeatherForecastDTO>>> Handle(ListWeatherForecastQuery request, CancellationToken cancellationToken)
  {
    var result = await _service.ListAsync();
    return Result.Success(result);
  }
}
