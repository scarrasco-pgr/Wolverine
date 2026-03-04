using Events;
using JasperFx;
using JasperFx.Events.Daemon;
using Marten;
using Marten.Events.Projections;
using Wolverine;
using Wolverine.API.Models;
using Wolverine.Http;
using Wolverine.Kafka;
using Wolverine.Marten;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("todo-db");
builder.Services.AddMarten(options =>
{
    options.Connection(connectionString!);
    options.DatabaseSchemaName = "todo";
    options.Projections.Snapshot<Todo>(SnapshotLifecycle.Inline);
}).UseLightweightSessions()
    .IntegrateWithWolverine()
    .AddAsyncDaemon(DaemonMode.HotCold)
    .PublishEventsToWolverine("Everything");

builder.Host.UseWolverine(opts =>
{
    var kafkaConnectionString = builder.Configuration.GetConnectionString("kafka");
    if (!string.IsNullOrEmpty(kafkaConnectionString))
    {
        opts.UseKafka(kafkaConnectionString).AutoProvision();
    }
    opts.PublishMessage<TodoCreated>().ToKafkaTopic("todo-events");
    opts.PublishMessage<TodoUpdated>().ToKafkaTopic("todo-events");
    opts.PublishMessage<TodoDeleted>().ToKafkaTopic("todo-events");
    opts.Policies.AutoApplyTransactions();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddWolverineHttp();

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapWolverineEndpoints(endpoint => endpoint.WarmUpRoutes = RouteWarmup.Eager);
app.UseHttpsRedirection();

return await app.RunJasperFxCommands(args);
