using CardCollections.Application.Interfaces;
using System.Collections.Concurrent;

namespace CardCollections.Infrastructure.Idempotency;

public sealed class InMemoryIdempotencyStore : IIdempotencyStore
{
    private readonly ConcurrentDictionary<string, bool> _keys = new();

    public Task<bool> ExistsAsync(string key)
    {
        return Task.FromResult(_keys.ContainsKey(key));
    }

    public Task StoreAsync(string key)
    {
        _keys.TryAdd(key, true);
        return Task.CompletedTask;
    }
}