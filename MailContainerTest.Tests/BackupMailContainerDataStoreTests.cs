using MailContainerTest.Data;
// ReSharper disable UnusedMember.Global

namespace MailContainerTest.Tests;

public class BackupMailContainerDataStoreTests : DataStoreTests<BackupMailContainerDataStore>
{
    protected override BackupMailContainerDataStore CreateDataStore() => new();
}