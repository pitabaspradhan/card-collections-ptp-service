using CardCollections.Application.Interfaces;
using CardCollections.Application.Services;
using CardCollections.Infrastructure.Events;
using CardCollections.Infrastructure.Idempotency;
using CardCollections.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
builder.Services.AddSingleton<ICollectionCaseRepository,
    InMemoryCollectionCaseRepository>();

builder.Services.AddSingleton<IIdempotencyStore,
    InMemoryIdempotencyStore>();

builder.Services.AddSingleton<IEventPublisher,
    FakeEventPublisher>();

builder.Services.AddScoped<ICollectionCaseService,
    CollectionCaseService>();

builder.Services.AddScoped<IPromiseToPayService,
    PromiseToPayService>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
