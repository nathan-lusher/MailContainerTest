using MailContainerTest.Data;
// ReSharper disable UnusedMember.Global

namespace MailContainerTest.Tests;

public class MailContainerDataStoreTests : DataStoreTests<MailContainerDataStore>
{
    protected override MailContainerDataStore CreateDataStore() => new();
}