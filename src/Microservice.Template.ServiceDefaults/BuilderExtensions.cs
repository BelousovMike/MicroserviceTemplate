using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Microservice.Template.ServiceDefaults;

/// <summary>
/// Добавляет общие сервисы .NET Aspire: service discovery, Resilience, Health checks и OpenTelemetry.
/// Этот проект должен быть добавлен в качестве зависимости к каждому сервисному проекту в вашем решении.
/// Подробнее о настройке Aspire: <see href="https://aka.ms/dotnet/aspire/service-defaults"/>.
/// </summary>
public static class BuilderExtensions
{
    /// <summary>
    /// Маппинг стандартных эндпоинтов.
    /// </summary>
    /// <param name="app">Конфигуратор роутов.</param>
    /// <returns>Конфигуратор с маппингом.</returns>
    public static WebApplication? MapDefaultEndpoints(this WebApplication? app)
    {
        ArgumentNullException.ThrowIfNull(app);
        if (!app.Environment.IsDevelopment())
        {
            return app;
        }

        // Все проверки должны пройти до запуска приложения, чтобы оно считалось работоспособным.
        app.MapHealthChecks("/health");

        // Чтобы приложение считалось активным, должны пройти только проверки работоспособности,
        // помеченные тегом "live"
        app.MapHealthChecks(
            "/alive",
            new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live"),
            });

        return app;
    }

    /// <summary>
    /// Метод-расширение, который добавляет в DI-контейнер стандартные сервисы для микросервисного приложения на .NET Aspire.
    /// </summary>
    /// <param name="builder">builder.</param>
    /// <typeparam name="TBuilder">Тип builder.</typeparam>
    /// <returns>Возвращает builder со стандартными сервисами.</returns>
    public static TBuilder AddServiceDefaults<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder.ConfigureOpenTelemetry();

        builder.AddDefaultHealthChecks();

        builder.Services.AddServiceDiscovery();

        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            // Включить устойчивость (resilience) по умолчанию
            http.AddStandardResilienceHandler();

            // Включить service discovery по умолчанию.
            http.AddServiceDiscovery();
        });

        return builder;
    }

    /// <summary>
    /// Конфигурирование Open Telemetry.
    /// </summary>
    /// <param name="builder">builder.</param>
    /// <typeparam name="TBuilder">тип builder.</typeparam>
    /// <returns>builder с настроенным Open Telemetry.</returns>
    private static TBuilder ConfigureOpenTelemetry<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder.Logging
            .AddOpenTelemetry(logging =>
            {
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;
            });

        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics => metrics.AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation())
            .WithTracing(tracing => tracing.AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation());

        builder.AddOpenTelemetryExporters();

        return builder;
    }

    /// <summary>
    /// Регистрация в приложении экспортеры для OpenTelemetry.
    /// </summary>
    /// <param name="builder">builder.</param>
    /// <typeparam name="TBuilder">Тип builder.</typeparam>
    /// <returns>builder с конфигурацией.</returns>
    private static TBuilder AddOpenTelemetryExporters<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]) ||
                              !string.IsNullOrWhiteSpace(builder.Configuration["DOTNET_DASHBOARD_OTLP_ENDPOINT_URL"]);

        if (useOtlpExporter)
        {
            builder.Services
                .AddOpenTelemetry()
                .UseOtlpExporter();
        }

        return builder;
    }

    /// <summary>
    /// Добавление Health Checks.
    /// </summary>
    /// <param name="builder">builder.</param>
    /// <typeparam name="TBuilder">Тип builder.</typeparam>
    /// <returns>builder с подключенным Health Checks.</returns>
    private static TBuilder AddDefaultHealthChecks<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }
}
