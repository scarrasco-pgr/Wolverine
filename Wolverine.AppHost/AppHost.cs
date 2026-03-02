var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume();

var pgadmin = postgres
    .WithPgWeb(p => p.WithParentRelationship(postgres));

var tododb = postgres.AddDatabase("todo-db");

var kafka = builder.AddKafka("kafka")
    .WithKafkaUI()
    .WithDataVolume("volume-kafka");

builder.AddProject<Projects.Wolverine_API>("wolverine-api")
    .WithReference(tododb)
    .WithReference(kafka)
    .WaitFor(tododb)
    .WaitFor(kafka);


builder.Build().Run();
