namespace CardCollections.Domain.Entities;

public sealed class AuditEntry
{
    public Guid Id { get; private set; }

    public string Action { get; private set; }

    public string User { get; private set; }

    public string CorrelationId { get; private set; }

    public DateTime TimestampUtc { get; private set; }

    private AuditEntry()
    {
    }

    public AuditEntry(
        string action,
        string user,
        string correlationId)
    {
        Id = Guid.NewGuid();
        Action = action;
        User = user;
        CorrelationId = correlationId;
        TimestampUtc = DateTime.UtcNow;
    }
}