using JasperFx;
using Marten;
using Wolverine;
using Wolverine.Http;
using Wolverine.Marten;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("todo-db") ?? throw new InvalidOperationException("Connection string 'todo-db' not found.");
builder.Services.AddMarten(options =>
{
    options.Connection(connectionString!);
    options.DatabaseSchemaName = "todo";
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
app.MapWolverineEndpoints();
app.UseHttpsRedirection();

return await app.RunJasperFxCommands(args);
