using System;
using System.Collections.Generic;
using System.Text;

namespace CardCollections.Application.Interfaces
{
    public interface IPromiseToPayService
    {
        Task<Guid> CreatePromiseToPayAsync(
            Guid caseId,
            decimal amount,
            DateOnly promiseDate,
            string agentId,
            string correlationId,
            string idempotencyKey);
    }
}
