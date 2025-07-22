using Microservice.Template.Infrastructure;

namespace Microservice.Template.Web.Configurations;

/// <summary>
/// Конфигурация сервисов.
/// </summary>
internal static class ServiceConfigs
{
    /// <summary>
    /// Добавляет конфигурацию сервисов в DI-контейнер.
    /// </summary>
    /// <param name="services">Коллекция сервисов для регистрации опций.</param>
    /// <param name="builder">Построитель веб-приложения для доступа к дополнительным сервисам.</param>
    /// <returns>
    /// <see cref="IServiceCollection"/> с зарегистрированными конфигурационными опциями.
    /// </returns>
    public static IServiceCollection AddServiceConfigs(
        this IServiceCollection services,
        WebApplicationBuilder builder)
        {
        services.AddInfrastructureServices(builder.Configuration)
            .AddMediatrConfigs();

        if (builder.Environment.IsDevelopment())
        {
            // === СЕРВИСЫ ТОЛЬКО ДЛЯ РАЗРАБОТКИ ===
            // Моки и заглушки для тестирования
            // services.AddScoped<IMockService, MockService>();

            // Отладочные сервисы
            // services.AddScoped<IDebugService, DebugService>();
        }

        // Сервисы, которые нужны в продакшене, но не в разработке
        // Например: реальные внешние API, продакшен кэш, мониторинг
        // services.AddScoped<IRealApiService, RealApiService>();
        return services;
    }
}
