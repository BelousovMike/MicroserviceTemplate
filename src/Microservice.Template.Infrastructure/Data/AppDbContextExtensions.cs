namespace Microservice.Template.Infrastructure.Data;

/// <summary>
/// Расширения для конфигурации контекста базы данных.
/// </summary>
public static class AppDbContextExtensions
{
    /// <summary>
    /// Добавляет контекст базы данных в DI-контейнер.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="connectionString">Строка подключения к базе данных.</param>
    /// <returns>Коллекция сервисов с добавленным контекстом базы данных.</returns>
    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
        return services;
    }
}
