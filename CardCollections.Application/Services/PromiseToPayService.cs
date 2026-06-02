using CardCollections.Application.DTOs;
using CardCollections.Application.Interfaces;
using CardCollections.Domain.Events;

namespace CardCollections.Application.Services;

public sealed class PromiseToPayService : IPromiseToPayService
{
    private readonly ICollectionCaseRepository _repository;
    private readonly IIdempotencyStore _idempotencyStore;
    private readonly IEventPublisher _eventPublisher;

    public PromiseToPayService(
        ICollectionCaseRepository repository,
        IIdempotencyStore idempotencyStore,
        IEventPublisher eventPublisher)
    {
        _repository = repository;
        _idempotencyStore = idempotencyStore;
        _eventPublisher = eventPublisher;
    }
    public async Task<PromiseToPayResponse> CreatePromiseToPayAsync(
    Guid caseId,
    decimal amount,
    DateOnly promiseDate,
    string agentId,
    string correlationId,
    string idempotencyKey)
    {
        // 1. Idempotency Check
        if (await _idempotencyStore.ExistsAsync(idempotencyKey))
        {
            throw new InvalidOperationException(
                "Duplicate request detected.");
        }

        // 2. Load Case
        var collectionCase =
            await _repository.GetByIdAsync(caseId);

        if (collectionCase is null)
        {
            throw new InvalidOperationException(
                $"Collection case '{caseId}' not found.");
        }

        // 3. Domain Operation
        var ptp = collectionCase.CreatePromiseToPay(
            amount,
            promiseDate,
            agentId,
            correlationId);

        // 4. Save Aggregate
        await _repository.SaveAsync(collectionCase);

        // 5. Publish Event
        var domainEvent =
            new PromiseToPayCreatedEvent(
                collectionCase.Id,
                ptp.Id,
                ptp.Amount,
                ptp.PromiseDate);

        await _eventPublisher.PublishAsync(domainEvent);

        // 6. Store Idempotency Key
        await _idempotencyStore.StoreAsync(idempotencyKey);

        return new PromiseToPayResponse
        {
            PromiseToPayId = ptp.Id,
            CaseId = collectionCase.Id,
            Status = collectionCase.Status.ToString(),
            Amount = ptp.Amount,
            PromiseDate = ptp.PromiseDate
        };
    }
}