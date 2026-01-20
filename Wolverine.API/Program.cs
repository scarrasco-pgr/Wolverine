using JasperFx;
using Marten;
using Marten.Events.Projections;
using Wolverine;
using Wolverine.API.Models;
using Wolverine.Http;
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
})
    .IntegrateWithWolverine();


builder.Host.UseWolverine(opts =>
{
    opts.Policies.AutoApplyTransactions();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddWolverineHttp();

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapWolverineEndpoints(endpoint => endpoint.WarmUpRoutes = RouteWarmup.Eager);
app.UseHttpsRedirection();

return await app.RunJasperFxCommands(args);
