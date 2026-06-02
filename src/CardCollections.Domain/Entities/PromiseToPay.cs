namespace CardCollections.Domain.Entities;

public sealed class PromiseToPay
{
    public Guid Id { get; private set; }

    public decimal Amount { get; private set; }

    public DateOnly PromiseDate { get; private set; }

    public string CreatedBy { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    private PromiseToPay()
    {
    }

    public PromiseToPay(
        decimal amount,
        DateOnly promiseDate,
        string createdBy)
    {
        Id = Guid.NewGuid();
        Amount = amount;
        PromiseDate = promiseDate;
        CreatedBy = createdBy;
        CreatedAtUtc = DateTime.UtcNow;
    }
}
