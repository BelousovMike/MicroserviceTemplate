using Microservice.Template.Infrastructure.Data;

namespace Microservice.Template.Web.Configurations;

/// <summary>
/// Конфигурация Middleware.
/// </summary>
internal static class MiddlewareConfig
{
    /// <summary>
    /// Конфигурирует Middleware и заполняет БД начальными данными.
    /// </summary>
    /// <param name="app">Экземпляр <see cref="WebApplication"/>, к которому применяется middleware и инициализация.</param>
    /// <returns>
    /// Асинхронная задача, результатом которой является <see cref="IApplicationBuilder"/> с применёнными изменениями.
    /// </returns>
    public static async Task<IApplicationBuilder> UseAppMiddlewareAndSeedDatabase(this WebApplication app)
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

        await SeedDatabase(app)
            .ConfigureAwait(false);

        return app;
    }

    /// <summary>
    /// Заполняет БД начальными значениями.
    /// </summary>
    /// <param name="app">Экземпляр <see cref="WebApplication"/>, к которому применяется middleware и инициализация.</param>
    private static async Task SeedDatabase(WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        IServiceProvider services = scope.ServiceProvider;

        try
        {
            AppDbContext context = services.GetRequiredService<AppDbContext>();
            await context.Database.MigrateAsync().ConfigureAwait(false);
            await context.Database.EnsureCreatedAsync().ConfigureAwait(false);

            // Раскоментировать при необходимости заполнения первоначальными данными
            // await SeedData.InitializeAsync(context);
        }
#pragma warning disable CA1031
        catch (Exception ex)
#pragma warning restore CA1031
        {
            ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Ошибка при заполнении БД. {ExceptionMessage}", ex.Message);
        }
    }
}
