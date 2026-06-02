using CardCollections.Api.Middleware;
using CardCollections.Application.Interfaces;
using CardCollections.Application.Services;
using CardCollections.Infrastructure.Events;
using CardCollections.Infrastructure.Idempotency;
using CardCollections.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Build AFTER all registrations
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();