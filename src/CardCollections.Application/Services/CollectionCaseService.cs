using CardCollections.Application.DTOs;
using CardCollections.Application.Interfaces;
using CardCollections.Domain.Entities;

namespace CardCollections.Application.Services;

public sealed class CollectionCaseService : ICollectionCaseService
{
    private readonly ICollectionCaseRepository _repository;

    public CollectionCaseService(
        ICollectionCaseRepository repository)
    {
        _repository = repository;
    }

    public async Task<CaseResponse> CreateCaseAsync(
        CreateCaseRequest request)
    {

        if (string.IsNullOrWhiteSpace(request.CustomerId))
            throw new InvalidOperationException(
                "CustomerId is required.");

        if (string.IsNullOrWhiteSpace(request.MaskedCardNumber))
            throw new InvalidOperationException(
                "MaskedCardNumber is required.");

        if (request.DelinquentAmount <= 0)
            throw new InvalidOperationException(
                "DelinquentAmount must be greater than zero.");

        var collectionCase = new CollectionCase(
            request.CustomerId,
            request.MaskedCardNumber,
            request.DelinquentAmount);

        await _repository.AddAsync(collectionCase);

        return new CaseResponse
        {
            CaseId = collectionCase.Id,
            CustomerId = collectionCase.CustomerId,
            MaskedCardNumber = collectionCase.MaskedCardNumber,
            DelinquentAmount = collectionCase.DelinquentAmount,
            Status = collectionCase.Status.ToString()
        };
    }

    public async Task<CaseResponse?> GetCaseAsync(
        Guid caseId)
    {
        var collectionCase =
            await _repository.GetByIdAsync(caseId);

        if (collectionCase is null)
        {
            return null;
        }

        return new CaseResponse
        {
            CaseId = collectionCase.Id,
            CustomerId = collectionCase.CustomerId,
            MaskedCardNumber = collectionCase.MaskedCardNumber,
            DelinquentAmount = collectionCase.DelinquentAmount,
            Status = collectionCase.Status.ToString()
        };
    }
}