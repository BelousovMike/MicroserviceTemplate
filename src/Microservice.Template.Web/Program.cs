using Microservice.Template.ServiceDefaults;
using Microservice.Template.UseCases;
using Microservice.Template.Web.Configurations;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddLoggerConfigs();

builder.Services.AddOptionConfigs(builder.Configuration, builder);
builder.Services.AddServiceConfigs(builder);

builder.Services.AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.Title = "Microservice Template API";
            s.Version = "v1";
            s.Description =
                "Документация для API микросервиса. Пример использования FastEndpoints и расширенного Swagger.";
        };
        o.ShortSchemaNames = true;
    })
    .AddCommandMiddleware(c => c.Register(typeof(QueryLogger<,>)));

// Подключить команды.
// builder.Services.AddTransient<ICommandHandler<AnyCommand,Result<int>>, AnyCommandHandler>();
#if aspire
builder.AddAspireDefaults();
#endif

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

app.ConfigureMiddleware();
await app.InitializeDatabaseAsync()
.ConfigureAwait(false);

await app.RunAsync()
    .ConfigureAwait(false);
