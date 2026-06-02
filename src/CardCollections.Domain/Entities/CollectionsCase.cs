using CardCollections.Domain.Enums;

namespace CardCollections.Domain.Entities;

public sealed class CollectionCase
{
    private readonly List<PromiseToPay> _promiseToPays = [];
    private readonly List<AuditEntry> _auditEntries = [];

    public Guid Id { get; private set; }

    public string CustomerId { get; private set; }

    public string MaskedCardNumber { get; private set; }

    public decimal DelinquentAmount { get; private set; }

    public CaseStatus Status { get; private set; }

    public IReadOnlyCollection<PromiseToPay> PromiseToPays =>
        _promiseToPays.AsReadOnly();

    public IReadOnlyCollection<AuditEntry> AuditEntries =>
        _auditEntries.AsReadOnly();

    private CollectionCase()
    {
    }

    public CollectionCase(
        string customerId,
        string maskedCardNumber,
        decimal delinquentAmount)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        MaskedCardNumber = maskedCardNumber;
        DelinquentAmount = delinquentAmount;
        Status = CaseStatus.Open;
    }
    public PromiseToPay CreatePromiseToPay(
    decimal amount,
    DateOnly promiseDate,
    string agentId,
    string correlationId)
    {
        if (amount <= 0)
        {
            throw new InvalidOperationException(
                "Promise amount must be greater than zero.");
        }

        if (promiseDate < DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new InvalidOperationException(
                "Promise date cannot be in the past.");
        }

        var ptp = new PromiseToPay(
            amount,
            promiseDate,
            agentId);

        _promiseToPays.Add(ptp);

        Status = CaseStatus.PromiseToPay;

        _auditEntries.Add(
            new AuditEntry(
                "PromiseToPayCreated",
                agentId,
                correlationId));

        return ptp;
    }
}