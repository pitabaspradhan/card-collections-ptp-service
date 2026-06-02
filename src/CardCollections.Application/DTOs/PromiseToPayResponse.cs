
namespace CardCollections.Application.DTOs
{
    public sealed class PromiseToPayResponse
    {
        public Guid PromiseToPayId { get; init; }

        public Guid CaseId { get; init; }

        public string Status { get; init; } = string.Empty;

        public decimal Amount { get; init; }

        public DateOnly PromiseDate { get; init; }
    }
}
