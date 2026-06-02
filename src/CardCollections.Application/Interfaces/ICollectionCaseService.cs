using CardCollections.Application.DTOs;

namespace CardCollections.Application.Interfaces;

public interface ICollectionCaseService
{
    Task<CaseResponse> CreateCaseAsync(
        CreateCaseRequest request);

    Task<CaseResponse?> GetCaseAsync(
        Guid caseId);
}