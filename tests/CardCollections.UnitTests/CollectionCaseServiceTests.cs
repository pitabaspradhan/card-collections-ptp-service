using CardCollections.Application.DTOs;
using CardCollections.Application.Services;
using CardCollections.Infrastructure.Repositories;

namespace CardCollections.UnitTests;


[TestClass]
public class CollectionCaseServiceTests
{
    [TestMethod]
    public async Task CreateCase_ShouldReturnCaseResponse()
    {
        // Arrange
        var repository =
            new InMemoryCollectionCaseRepository();

        var service =
            new CollectionCaseService(repository);

        var request =
            new CreateCaseRequest
            {
                CustomerId = "CUST001",
                MaskedCardNumber = "XXXX-XXXX-XXXX-1234",
                DelinquentAmount = 25000
            };

        // Act
        var result =
            await service.CreateCaseAsync(request);

        // Assert
        Assert.IsNotNull(result);

        Assert.AreEqual(
            "CUST001",
            result.CustomerId);

        Assert.AreEqual(
            "XXXX-XXXX-XXXX-1234",
            result.MaskedCardNumber);

        Assert.AreEqual(
            25000,
            result.DelinquentAmount);

        Assert.AreEqual(
            "Open",
            result.Status);

        Assert.AreNotEqual(
            Guid.Empty,
            result.CaseId);
    }
}