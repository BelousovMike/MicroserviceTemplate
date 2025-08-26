using Microservice.Template.Infrastructure.Data;

namespace Microservice.Template.Web.Configurations;

/// <summary>
/// Конфигурация Middleware.
/// </summary>
internal static class MiddlewareConfig
{
    /// <summary>
    /// Настраивает middleware приложения.
    /// </summary>
    /// <param name="app">Экземпляр веб-приложения.</param>
    /// <returns>Веб-приложение с настроенным middleware.</returns>
    public static WebApplication ConfigureMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseShowAllServicesMiddleware(); // см. https://github.com/ardalis/AspNetCoreStartupServices
        }
        else
        {
            app.UseDefaultExceptionHandler(); // из FastEndpoints
            app.UseHsts();
        }

        app.UseFastEndpoints()
            .UseSwaggerGen(); // включает в себя AddFileServer и static files middleware

        app.UseHttpsRedirection(); // Внимание: при этом удаляются заголовки авторизации!!!!

        return app;
    }

    /// <summary>
    /// Применяет миграции базы данных.
    /// </summary>
    /// <param name="app">Экземпляр веб-приложения.</param>
    /// <returns>Веб-приложение с применёнными миграциями.</returns>
    public static async Task<WebApplication> ApplyDatabaseMigrationsAsync(this WebApplication app)
    {
        IServiceProvider provider = app.GetServiceProvider();
        try
        {
            AppDbContext context = provider.GetRequiredService<AppDbContext>();
            await context.Database.MigrateAsync().ConfigureAwait(false);
            await context.Database.EnsureCreatedAsync().ConfigureAwait(false);
        }
#pragma warning disable CA1031
        catch (Exception ex)
#pragma warning restore CA1031
        {
            ILogger<Program> logger = provider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Ошибка при применении миграций БД. {ExceptionMessage}", ex.Message);
        }

        return app;
    }

    /// <summary>
    /// Заполняет базу данных первоначальными данными.
    /// </summary>
    /// <param name="app">Экземпляр веб-приложения.</param>
    /// <returns>Веб-приложение с заполненной базой данных.</returns>
    public static async Task<WebApplication> SeedInitialDataAsync(this WebApplication app)
    {
        IServiceProvider provider = app.GetServiceProvider();
        try
        {
            // Раскомментировать при необходимости заполнения первоначальными данными
            // AppDbContext context = provider.GetRequiredService<AppDbContext>();
            // await SeedData.InitializeAsync(context).ConfigureAwait(false);
            // Имитация полезной нагрузки, вместо реальной работы.
            await Task.Delay(1).ConfigureAwait(false);
        }
#pragma warning disable CA1031
        catch (Exception ex)
#pragma warning restore CA1031
        {
            ILogger<Program> logger = provider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Ошибка при заполнении БД первоначальными данными. {ExceptionMessage}", ex.Message);
        }

        return app;
    }

    /// <summary>
    /// Инициализирует базу данных: применяет миграции и заполняет первоначальными данными.
    /// </summary>
    /// <param name="app">Экземпляр веб-приложения.</param>
    /// <returns>Веб-приложение с инициализированной базой данных.</returns>
    public static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
    {
        await app.ApplyDatabaseMigrationsAsync().ConfigureAwait(false);
        await app.SeedInitialDataAsync().ConfigureAwait(false);
        return app;
    }

    /// <summary>
    /// Получает провайдер сервисов из области действия приложения.
    /// </summary>
    /// <param name="app">Веб-приложение.</param>
    /// <returns>Провайдер сервисов.</returns>
    private static IServiceProvider GetServiceProvider(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        return scope.ServiceProvider;
    }
}
