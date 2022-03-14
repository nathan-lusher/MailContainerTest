using MailContainerTest.Data;
using MailContainerTest.Types;
using Xunit;

namespace MailContainerTest.Tests;

public abstract class DataStoreTests<T> where T : IMailContainerDataStore
{
    [Fact]
    public void GetMailContainerReturnsItem()
    {
        IMailContainerDataStore dataStore = CreateDataStore();

        Assert.NotNull(dataStore.GetMailContainer(Guid.NewGuid().ToString()));
    }

    [Fact]
    public void UpdateMailContainerDoesNotThrowException()
    {
        IMailContainerDataStore dataStore = CreateDataStore();

        dataStore.UpdateMailContainer(new MailContainer());
    }

    protected abstract T CreateDataStore();
}