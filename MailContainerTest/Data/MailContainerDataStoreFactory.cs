using MailContainerTest.Types;

namespace MailContainerTest.Data;

/// <inheritdoc />
public class MailContainerDataStoreFactory : IMailContainerDataStoreFactory
{
    /// <inheritdoc />
    public IMailContainerDataStore Create(MailContainerDataStoreType dataStoreType)
    {
        return dataStoreType == MailContainerDataStoreType.Backup
            ? new BackupMailContainerDataStore()
            : new MailContainerDataStore();
    }
}