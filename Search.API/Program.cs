using JasperFx.Core;
using Search.API.Data.Contexts;
using Wolverine;
using Wolverine.ErrorHandling;
using Wolverine.Kafka;

var builder = WebApplication.CreateBuilder(args);

builder.AddNpgsqlDbContext<TodoContext>(connectionName: "search-db");
builder.AddServiceDefaults();
builder.Services.AddOpenApi();
builder.Host.UseWolverine(opts =>
{
    var kafkaConnectionString = builder.Configuration.GetConnectionString("kafka");
    if (!string.IsNullOrEmpty(kafkaConnectionString))
    {
        opts.UseKafka(kafkaConnectionString);
    }
    opts.ListenToKafkaTopic("todo-events");

    // Error handling policy for all handlers in this project
    opts.OnException<HttpRequestException>()
        .RetryWithCooldown(50.Milliseconds(), 200.Milliseconds(), 1.Seconds());
});
var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapDefaultEndpoints();

app.Run();
