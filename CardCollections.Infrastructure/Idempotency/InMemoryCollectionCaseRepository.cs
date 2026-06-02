using CardCollections.Application.Interfaces;
using CardCollections.Domain.Entities;
using System.Collections.Concurrent;

namespace CardCollections.Infrastructure.Repositories;

public sealed class InMemoryCollectionCaseRepository
    : ICollectionCaseRepository
{
    private readonly ConcurrentDictionary<Guid, CollectionCase> _store = new();

    public Task AddAsync(CollectionCase collectionCase)
    {
        _store.TryAdd(collectionCase.Id, collectionCase);
        return Task.CompletedTask;
    }

    public Task<CollectionCase?> GetByIdAsync(Guid caseId)
    {
        _store.TryGetValue(caseId, out var collectionCase);
        return Task.FromResult(collectionCase);
    }

    public Task SaveAsync(CollectionCase collectionCase)
    {
        _store[collectionCase.Id] = collectionCase;
        return Task.CompletedTask;
    }
}