using System.Diagnostics;
using System.Reflection;
using FastEndpoints;
using Microsoft.Extensions.Logging;

namespace MicroserviceTemplate.UseCases;

public sealed class QueryLogger<TCommand, TResult>(ILogger<TCommand> logger)
  : ICommandMiddleware<TCommand, TResult> where TCommand : ICommand<TResult>
{
  public async Task<TResult> ExecuteAsync(TCommand command,
    CommandDelegate<TResult> next,
    CancellationToken ct)
  {
    string commandName = command.GetType().Name;
    if (logger.IsEnabled(LogLevel.Information))
    {
      logger.LogInformation("Handling {RequestName}", commandName);

      // Рефлексия! Могут быть проблемы с производительностью.
      Type myType = command.GetType();
      IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
      foreach (PropertyInfo prop in props)
      {
        object? propValue = prop?.GetValue(command, null);
        logger.LogInformation("Property {Property} : {@Value}", prop?.Name, propValue);
      }
    }

    var sw = Stopwatch.StartNew();

    var result = await next();

    logger.LogInformation("Handled {CommandName} with {Result} in {ms} ms", commandName, result, sw.ElapsedMilliseconds);
    sw.Stop();

    return result;
  }
}
