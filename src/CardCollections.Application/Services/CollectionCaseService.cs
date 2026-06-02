using CardCollections.Application.DTOs;
using CardCollections.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardCollections.Application.Services
{
    public class CollectionCaseService : ICollectionCaseService
    {
        public Task<CaseResponse> CreateCaseAsync(CreateCaseRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<CaseResponse?> GetCaseAsync(Guid caseId)
        {
            throw new NotImplementedException();
        }
    }
}
