var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume();

var pgadmin = postgres
    .WithPgWeb(p => p.WithParentRelationship(postgres));

var tododb = postgres.AddDatabase("todo-db");
var searchDb = postgres.AddDatabase("search-db");

var migrations = builder.AddProject<Projects.Search_API_Migration>("migrations")
    .WithReference(searchDb)
    .WaitFor(searchDb);

var kafka = builder.AddKafka("kafka")
    .WithKafkaUI()
    .WithDataVolume("volume-kafka");

builder.AddProject<Projects.Wolverine_API>("wolverine-api")
    .WithReference(tododb)
    .WithReference(kafka)
    .WaitFor(tododb)
    .WaitFor(kafka);


builder.AddProject<Projects.Search_API>("search-api")
    .WithReference(searchDb)
    .WithReference(kafka)
    .WithReference(migrations)
    .WaitFor(migrations)
    .WaitFor(searchDb)
    .WaitFor(kafka);


builder.Build().Run();
