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
    public static void AddApplicationDbContext(this IServiceCollection services, string connectionString) =>
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));
}
