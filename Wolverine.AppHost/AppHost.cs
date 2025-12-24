var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Wolverine_API>("wolverine-api");

builder.Build().Run();
