var builder = DistributedApplication.CreateBuilder(args);
builder.AddProject<Projects.Microservice_Template_Web>("web");
await builder.Build()
  .RunAsync();
