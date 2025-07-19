namespace Microservice.Template.Web.Configurations;

/// <summary>
/// Конфигурация логгера в DI.
/// </summary>
internal static class LoggerConfigs
{
    /// <summary>
    /// Добавление логгера в DI.
    /// </summary>
    /// <param name="builder">Экземпляр <see cref="WebApplicationBuilder"/>, используемый для конфигурирования сервисов и параметров приложения.</param>
    /// <returns><see cref="WebApplicationBuilder"/> с добавленным Serilog.</returns>
    public static WebApplicationBuilder AddLoggerConfigs(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));
        return builder;
    }
}
