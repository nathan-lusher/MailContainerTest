using MailContainerTest.Data;
using MailContainerTest.Types;
using Xunit;

namespace MailContainerTest.Tests;

public class MailDataStoreFactoryTests
{
    private static string NewGuid => Guid.NewGuid().ToString();

    [Theory]
    [InlineData(MailContainerDataStoreType.Standard, typeof(MailContainerDataStore))]
    [InlineData(MailContainerDataStoreType.Backup, typeof(BackupMailContainerDataStore))]
    public void CorrectDataStoreTypeIsCreated(MailContainerDataStoreType dataStoreType, Type expectedType)
    {
        IMailContainerDataStore dataStore = CreateDataStoreFromFactory(dataStoreType);

        Assert.IsType(expectedType, dataStore);
    }

    [Theory]
    [InlineData(MailContainerDataStoreType.Standard)]
    [InlineData(MailContainerDataStoreType.Backup)]
    public void CreatedContainerHasCorrectType(MailContainerDataStoreType dataStoreType)
    {
        IMailContainerDataStore dataStore = CreateDataStoreFromFactory(dataStoreType);

        Assert.Equal(dataStoreType, dataStore.GetMailContainer(NewGuid).ContainerDataStoreType);
    }

    [Theory]
    [InlineData(MailContainerDataStoreType.Standard)]
    [InlineData(MailContainerDataStoreType.Backup)]
    public void CreatedContainerHasCorrectNumber(MailContainerDataStoreType dataStoreType)
    {
        string containerNumber = NewGuid;
        IMailContainerDataStore dataStore = CreateDataStoreFromFactory(dataStoreType);

        Assert.Equal(containerNumber, dataStore.GetMailContainer(containerNumber).MailContainerNumber);
    }

    private static IMailContainerDataStore CreateDataStoreFromFactory(MailContainerDataStoreType dataStoreType)
    {
        IMailContainerDataStoreFactory factory = new MailContainerDataStoreFactory();
        return factory.Create(dataStoreType);
    }
}