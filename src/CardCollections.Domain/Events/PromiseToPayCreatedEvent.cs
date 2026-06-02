namespace CardCollections.Domain.Events;

public sealed class PromiseToPayCreatedEvent
{
    public Guid CaseId { get; }

    public Guid PromiseToPayId { get; }

    public decimal Amount { get; }

    public DateOnly PromiseDate { get; }

    public DateTime OccurredAtUtc { get; }

    public PromiseToPayCreatedEvent(
        Guid caseId,
        Guid promiseToPayId,
        decimal amount,
        DateOnly promiseDate)
    {
        CaseId = caseId;
        PromiseToPayId = promiseToPayId;
        Amount = amount;
        PromiseDate = promiseDate;
        OccurredAtUtc = DateTime.UtcNow;
    }
}