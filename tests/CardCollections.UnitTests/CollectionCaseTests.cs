using CardCollections.Domain.Entities;
using CardCollections.Domain.Enums;

namespace CardCollections.UnitTests.Domain;

[TestClass]
public class CollectionCaseTests
{
    [TestMethod]
    public void CreatePromiseToPay_ShouldChangeStatus()
    {
        // Arrange
        var collectionCase = new CollectionCase(
            "CUST001",
            "XXXX-XXXX-XXXX-1234",
            25000);

        // Act
        collectionCase.CreatePromiseToPay(
            10000,
            DateOnly.FromDateTime(DateTime.Today.AddDays(7)),
            "AGENT001", "Corrl-001");

        // Assert
        Assert.AreEqual(
            CaseStatus.PromiseToPay,
            collectionCase.Status);
    }

    [TestMethod]
    public void CreatePromiseToPay_ShouldAddPromiseToPay()
    {
        var collectionCase = new CollectionCase(
            "CUST001",
            "XXXX-XXXX-XXXX-1234",
            25000);

        collectionCase.CreatePromiseToPay(
            10000,
            DateOnly.FromDateTime(DateTime.Today.AddDays(7)),
            "AGENT002", "Corrl-002");

        Assert.AreEqual(
            1,
            collectionCase.PromiseToPays.Count);
    }

    [TestMethod]
    public void CreatePromiseToPay_ShouldCreateAuditEntry()
    {
        var collectionCase = new CollectionCase(
            "CUST001",
            "XXXX-XXXX-XXXX-1234",
            25000);

        collectionCase.CreatePromiseToPay(
            10000,
            DateOnly.FromDateTime(DateTime.Today.AddDays(7)),
            "AGENT003", "Corrl-003");

        Assert.AreEqual(
            1,
            collectionCase.AuditEntries.Count);
    }
}