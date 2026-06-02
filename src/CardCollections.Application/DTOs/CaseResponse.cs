
namespace CardCollections.Application.DTOs
{
    public sealed class CaseResponse
    {
        public Guid CaseId { get; init; }

        public string CustomerId { get; init; } = string.Empty;

        public string MaskedCardNumber { get; init; } = string.Empty;

        public decimal DelinquentAmount { get; init; }

        public string Status { get; init; } = string.Empty;
    }
}
