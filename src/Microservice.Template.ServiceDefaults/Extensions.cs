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

// Adds common .NET Aspire services: service discovery, resilience, health checks, and OpenTelemetry.
// This project should be referenced by each service project in your solution.
// To learn more about using this project, see https://aka.ms/dotnet/aspire/service-defaults
public static class Extensions
{
  public static TBuilder AddServiceDefaults<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
  {
    builder.ConfigureOpenTelemetry();

    builder.AddDefaultHealthChecks();

    builder.Services.AddServiceDiscovery();

    builder.Services.ConfigureHttpClientDefaults(http =>
    {
      // Turn on resilience by default
      http.AddStandardResilienceHandler();

      // Turn on service discovery by default
      http.AddServiceDiscovery();
    });

    // Uncomment the following to restrict the allowed schemes for service discovery.
    // builder.Services.Configure<ServiceDiscoveryOptions>(options =>
    // {
    //     options.AllowedSchemes = ["https"];
    // });

    return builder;
  }

  public static TBuilder ConfigureOpenTelemetry<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
  {
    builder.Logging.AddOpenTelemetry(logging =>
    {
      logging.IncludeFormattedMessage = true;
      logging.IncludeScopes = true;
    });

    builder.Services.AddOpenTelemetry()
        .WithMetrics(metrics =>
        {
          metrics.AddAspNetCoreInstrumentation()
                  .AddHttpClientInstrumentation()
                  .AddRuntimeInstrumentation();
        })
        .WithTracing(tracing =>
        {
          tracing.AddAspNetCoreInstrumentation()
                  // Uncomment the following line to enable gRPC instrumentation (requires the OpenTelemetry.Instrumentation.GrpcNetClient package)
                  //.AddGrpcClientInstrumentation()
                  .AddHttpClientInstrumentation();
        });

    builder.AddOpenTelemetryExporters();

    return builder;
  }

  private static TBuilder AddOpenTelemetryExporters<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
  {
    var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]) ||
                          !string.IsNullOrWhiteSpace(builder.Configuration["DOTNET_DASHBOARD_OTLP_ENDPOINT_URL"]);

    if (useOtlpExporter)
    {
      builder.Services.AddOpenTelemetry().UseOtlpExporter();
    }

    // Раскомментировать для включения Azure Monitor exporter
    // (требуется пакет Azure.Monitor.OpenTelemetry.AspNetCore)
    //if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
    //{
    //    builder.Services.AddOpenTelemetry()
    //       .UseAzureMonitor();
    //}

    return builder;
  }

  public static TBuilder AddDefaultHealthChecks<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
  {
    builder.Services.AddHealthChecks()
        .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

    return builder;
  }

  public static WebApplication MapDefaultEndpoints(this WebApplication app)
  {
    if (app.Environment.IsDevelopment())
    {
      // Все проверки должны пройти до запуска приложения, чтобы оно считалось работоспособным.
      app.MapHealthChecks("/health");

      // Чтобы приложение считалось активным, должны пройти только проверки работоспособности,
      // помеченные тегом "live"
      app.MapHealthChecks("/alive", new HealthCheckOptions
      {
        Predicate = r => r.Tags.Contains("live")
      });
    }

    return app;
  }
}
