using CardCollections.Application.Interfaces;

namespace CardCollections.Infrastructure.Events;

public sealed class FakeEventPublisher : IEventPublisher
{
    public Task PublishAsync<T>(T @event)
    {
        Console.WriteLine(
            $"Event Published: {typeof(T).Name}");

        return Task.CompletedTask;
    }
}