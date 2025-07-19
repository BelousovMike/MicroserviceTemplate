using System.Diagnostics;
using System.Reflection;
using FastEndpoints;
using Microsoft.Extensions.Logging;

namespace Microservice.Template.UseCases;

/// <summary>
/// Логгирование запросов.
/// </summary>
/// <param name="logger">Логгер.</param>
/// <typeparam name="TCommand">Тип команды.</typeparam>
/// <typeparam name="TResult">Тип результата.</typeparam>
public sealed class QueryLogger<TCommand, TResult>(ILogger<TCommand> logger)
    : ICommandMiddleware<TCommand, TResult>
    where TCommand : ICommand<TResult>
    {
    /// <summary>
    /// Метод обработки.
    /// </summary>
    /// <param name="command">Команда.</param>
    /// <param name="next">Следующее действие в конвейере.</param>
    /// <param name="ct">Токен отмены.</param>
    /// <returns>
    /// <see cref="Task{TResult}"/> представляющий асинхронную операцию.
    /// Результат содержит объект типа <typeparamref name="TResult"/>.
    /// </returns>
    public async Task<TResult> ExecuteAsync(
        TCommand command,
        CommandDelegate<TResult> next,
        CancellationToken ct)
        {
        var commandName = command.GetType().Name;
        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Обработка {RequestName}", commandName);

            // Рефлексия! Могут быть проблемы с производительностью.
            Type myType = command.GetType();
            IList<PropertyInfo> props = [.. myType.GetProperties()];
            foreach (PropertyInfo? prop in props)
            {
                var propValue = prop?.GetValue(command, null);
                logger.LogInformation("Property {Property} : {@Value}", prop?.Name, propValue);
            }
        }

        var sw = Stopwatch.StartNew();
        ArgumentNullException.ThrowIfNull(next);

        TResult result = await next()
            .ConfigureAwait(false);

        logger.LogInformation(
            "Handled {CommandName} with {Result} in {Ms} ms",
            commandName,
            result,
            sw.ElapsedMilliseconds);
        sw.Stop();

        return result;
    }
}
