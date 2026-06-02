using CardCollections.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardCollections.Application.Interfaces
{
    public interface ICollectionCaseRepository
    {
        Task<CollectionCase?> GetByIdAsync(Guid caseId);

        Task AddAsync(CollectionCase collectionCase);

        Task SaveAsync(CollectionCase collectionCase);
    }
}
