using MicroserviceTemplate.ServiceDefaults;
using MicroserviceTemplate.UseCases;
using MicroserviceTemplate.Web.Configurations;

var builder = WebApplication.CreateBuilder(args);

var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

logger.Information("Starting web host");

builder.AddLoggerConfigs();

var appLogger = new SerilogLoggerFactory(logger)
  .CreateLogger<MicroserviceTemplate.Web.Program>();

builder.Services.AddOptionConfigs(builder.Configuration, appLogger, builder);
builder.Services.AddServiceConfigs(appLogger, builder);


builder.Services.AddFastEndpoints()
  .SwaggerDocument(o =>
  {
    o.ShortSchemaNames = true;
  })
  .AddCommandMiddleware(c =>
  {
    c.Register(typeof(QueryLogger<,>));
  });

// Подключить команды.
// builder.Services.AddTransient<ICommandHandler<AnyCommand,Result<int>>, AnyCommandHandler>();

#if (aspire)
builder.AddServiceDefaults();
#endif

var app = builder.Build();

await app.UseAppMiddlewareAndSeedDatabase();

await app.RunAsync();

// Делает класс Program.cs публичным, чтобы интеграционные тесты ссылались на правильную сборку при собрке хоста.
namespace MicroserviceTemplate.Web
{
  public partial class Program { }
}
