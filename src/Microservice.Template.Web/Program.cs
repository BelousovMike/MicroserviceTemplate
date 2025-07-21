using Microservice.Template.ServiceDefaults;
using Microservice.Template.UseCases;
using Microservice.Template.Web.Configurations;
using ILogger = Microsoft.Extensions.Logging.ILogger;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Serilog.ILogger logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
  .CreateLogger();

logger.Information("Starting web host");

builder.AddLoggerConfigs();

using var loggerFactory = new SerilogLoggerFactory(logger);
ILogger appLogger = loggerFactory.CreateLogger("Microservice.Template.Web.Program");

builder.Services.AddOptionConfigs(builder.Configuration, appLogger, builder);
builder.Services.AddServiceConfigs(appLogger, builder);

builder.Services.AddFastEndpoints()
  .SwaggerDocument(o => o.ShortSchemaNames = true)
  .AddCommandMiddleware(c => c.Register(typeof(QueryLogger<,>)));

// Подключить команды.
// builder.Services.AddTransient<ICommandHandler<AnyCommand,Result<int>>, AnyCommandHandler>();

#if aspire
builder.AddServiceDefaults();
#endif

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

await app.UseAppMiddlewareAndSeedDatabase()
    .ConfigureAwait(false);

await app.RunAsync()
    .ConfigureAwait(false);
