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
    /// <param name="services">Коллекция сервисов для регистрации.</param>
    /// <param name="builder">Построитель веб-приложения.</param>
    /// <returns>
    /// Коллекция сервисов с зарегистрированными конфигурационными опциями.
    /// </returns>
    public static IServiceCollection AddServiceConfigs(
        this IServiceCollection services,
        WebApplicationBuilder builder) =>
        services
            .AddInfrastructureServices(builder.Configuration)
            .AddDevelopmentOnlyServices(builder)
            .AddProductionOnlyServices()
            .AddMediatrConfigs();

    /// <summary>
    /// Добавляет сервисы только для режима разработки.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="builder">Построитель веб-приложения.</param>
    /// <returns>Коллекция сервисов с добавленными отладочными сервисами.</returns>
    private static IServiceCollection AddDevelopmentOnlyServices(this IServiceCollection services, WebApplicationBuilder builder) =>
        !builder.Environment.IsDevelopment() ? services :

            // === СЕРВИСЫ ТОЛЬКО ДЛЯ РАЗРАБОТКИ ===
            // Моки и заглушки для тестирования
            // services.AddScoped<IMockService, MockService>();
            // Отладочные сервисы
            // services.AddScoped<IDebugService, DebugService>();
            services;

    /// <summary>
    /// Добавляет сервисы только для продакшн режима.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Коллекция сервисов с добавленными продакшн сервисами.</returns>
    private static IServiceCollection AddProductionOnlyServices(this IServiceCollection services) =>

        // Сервисы, которые нужны в продакшене, но не в разработке
        // Например: реальные внешние API, продакшен кэш, мониторинг
        // services.AddScoped<IRealApiService, RealApiService>();
        services;
}
