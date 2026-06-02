

namespace CardCollections.Application.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T @event);
    }
}
