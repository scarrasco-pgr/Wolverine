using JasperFx.Core;
using Microsoft.EntityFrameworkCore;
using Search.API.Data.Contexts;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.ErrorHandling;
using Wolverine.Kafka;
using Wolverine.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddNpgsqlDbContext<TodoContext>("search-db");
builder.AddServiceDefaults();
builder.Services.AddOpenApi();
builder.Host.UseWolverine(opts =>
{
    var connectionString = builder.Configuration.GetConnectionString("search-db");
    opts.UseEntityFrameworkCoreTransactions(TransactionMiddlewareMode.Lightweight);
    opts.Policies.UseDurableLocalQueues();

    var kafkaConnectionString = builder.Configuration.GetConnectionString("kafka");
    if (!string.IsNullOrEmpty(kafkaConnectionString))
    {
        opts.UseKafka(kafkaConnectionString);
    }
    opts.ListenToKafkaTopic("todo-events");
    opts.OnException<HttpRequestException>()
        .RetryWithCooldown(50.Milliseconds(), 200.Milliseconds(), 1.Seconds());
});
var app = builder.Build();

app.MapGet("/search", async (string? query, TodoContext db) =>
{
    if (query is null)
    {
        return Results.Ok(await db.Todos.ToListAsync());
    }

    var results = await db.Todos
        .Where(t => EF.Functions.TrigramsSimilarity(t.Description, query) > 0.3)
        .OrderByDescending(t => EF.Functions.TrigramsSimilarity(t.Description, query))
        .ToListAsync();

    return Results.Ok(results);
});

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapDefaultEndpoints();

app.Run();
