namespace CardCollections.Application.DTOs
{
    public sealed class CreateCaseRequest
    {
        public string CustomerId { get; init; } = string.Empty;

        public string MaskedCardNumber { get; init; } = string.Empty;

        public decimal DelinquentAmount { get; init; }
    }
}
