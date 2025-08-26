namespace Microservice.Template.Web.Configurations;

/// <summary>
/// Конфигурация логгера в DI.
/// </summary>
internal static class LoggerConfigs
{
    /// <summary>
    /// Добавляет конфигурацию логирования в приложение.
    /// </summary>
    /// <param name="builder">Построитель веб-приложения.</param>
    /// <returns>Построитель с добавленной конфигурацией логирования.</returns>
    public static WebApplicationBuilder AddLoggerConfigs(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));
        return builder;
    }
}
