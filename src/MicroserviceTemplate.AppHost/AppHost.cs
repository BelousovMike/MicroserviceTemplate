var builder = DistributedApplication.CreateBuilder(args);
builder.AddProject<Projects.MicroserviceTemplate_Web>("web");
await builder.Build()
  .RunAsync();
