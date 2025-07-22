namespace Microservice.Template.Web.Configurations;

/// <summary>
/// Настройка опций.
/// </summary>
internal static class OptionConfigs
{
    /// <summary>
    /// Добавляет конфигурационные опции в DI-контейнер.
    /// </summary>
    /// <param name="services">Коллекция сервисов для регистрации опций.</param>
    /// <param name="configuration">Конфигурация приложения для чтения настроек.</param>
    /// <param name="builder">Построитель веб-приложения для доступа к дополнительным сервисам.</param>
    /// <returns>
    /// <see cref="IServiceCollection"/> с зарегистрированными конфигурационными опциями.
    /// </returns>
    public static IServiceCollection AddOptionConfigs(
        this IServiceCollection services,
        IConfiguration configuration,
        WebApplicationBuilder builder)
        {
        // конфигурация сервисов.
        // services.AddScoped<IAnyService, AnyService>();

        // Настройка Web Behavior.
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = _ => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        if (builder.Environment.IsDevelopment())
        {
            // Добавить список сервисов для диагностики.
            // - see https://github.com/ardalis/AspNetCoreStartupServices
            services.Configure<ServiceConfig>(config =>
            {
                config.Services = new List<ServiceDescriptor>(builder.Services);

                // optional - стандартный путь для просмотра сервисов /listallservices
                // - рекомендуется указывать собственный путь.
                config.Path = "/listservices";
            });
        }

        return services;
    }
}
