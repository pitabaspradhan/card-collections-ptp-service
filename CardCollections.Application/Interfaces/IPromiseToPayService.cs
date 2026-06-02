
using CardCollections.Application.DTOs;

namespace CardCollections.Application.Interfaces
{
    public interface IPromiseToPayService
    {
        Task<PromiseToPayResponse> CreatePromiseToPayAsync(
            Guid caseId,
            decimal amount,
            DateOnly promiseDate,
            string agentId,
            string correlationId,
            string idempotencyKey);
    }
}
