var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume();

var pgadmin = postgres.WithPgAdmin(p => p.WithParentRelationship(postgres))
    .WithPgWeb(p => p.WithParentRelationship(postgres));

var tododb = postgres.AddDatabase("todo-db");

builder.AddProject<Projects.Wolverine_API>("wolverine-api")
    .WithReference(tododb)
    .WaitFor(tododb);

builder.Build().Run();
