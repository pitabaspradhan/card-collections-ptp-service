
namespace CardCollections.Application.DTOs
{
    public sealed class CreatePromiseToPayRequest
    {
        public decimal Amount { get; init; }

        public DateOnly PromiseDate { get; init; }

        public string AgentId { get; init; } = string.Empty;
    }
}
