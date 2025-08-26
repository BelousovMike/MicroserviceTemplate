namespace Microservice.Template.Web.Configurations;

/// <summary>
/// Настройка опций.
/// </summary>
internal static class OptionConfigs
{
    /// <summary>
    /// Добавляет конфигурационные опции в DI-контейнер.
    /// </summary>
    /// <param name="services">Коллекция сервисов для регистрации.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <param name="builder">Построитель веб-приложения.</param>
    /// <returns>
    /// Коллекция сервисов с зарегистрированными конфигурационными опциями.
    /// </returns>
    public static IServiceCollection AddOptionConfigs(
        this IServiceCollection services,
        IConfiguration configuration,
        WebApplicationBuilder builder) =>
        services
            .AddBaseServices()
            .ConfigureCookiePolicy()
            .ConfigureDevelopmentServices(builder);

    /// <summary>
    /// Добавляет базовые сервисы в коллекцию.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Коллекция сервисов с базовыми сервисами.</returns>
    private static IServiceCollection AddBaseServices(this IServiceCollection services) =>
        services;

    /// <summary>
    /// Настраивает политику cookies для веб-приложения.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Коллекция сервисов с настроенной политикой cookies.</returns>
    private static IServiceCollection ConfigureCookiePolicy(this IServiceCollection services) =>
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = _ => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

    /// <summary>
    /// Настраивает сервисы для отображения списка сервисов в режиме разработки.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="builder">Построитель веб-приложения.</param>
    /// <returns>Коллекция сервисов с настроенными сервисами для отображения списка.</returns>
    private static IServiceCollection ConfigureDevelopmentServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            services.Configure<ServiceConfig>(config =>
            {
                config.Services = [.. builder.Services];

                // optional - стандартный путь для просмотра сервисов /listallservices
                // - рекомендуется указывать собственный путь.
                config.Path = "/listservices";
            });
        }

        return services;
    }
}
