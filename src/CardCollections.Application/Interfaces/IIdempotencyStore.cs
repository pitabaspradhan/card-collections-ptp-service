using System;
using System.Collections.Generic;
using System.Text;

namespace CardCollections.Application.Interfaces
{
    public interface IIdempotencyStore
    {
        Task<bool> ExistsAsync(string key);

        Task StoreAsync(string key);
    }
}
