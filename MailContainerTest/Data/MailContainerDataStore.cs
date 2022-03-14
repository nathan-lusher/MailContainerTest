using MailContainerTest.Types;

namespace MailContainerTest.Data;

/// <inheritdoc />
public class MailContainerDataStore : IMailContainerDataStore
{
    /// <inheritdoc />
    public MailContainer GetMailContainer(string mailContainerNumber)
    {
        // Access the database and return the retrieved mail container. Implementation not required for this exercise.
        return new MailContainer { ContainerDataStoreType = MailContainerDataStoreType.Standard, MailContainerNumber = mailContainerNumber };
    }

    /// <inheritdoc />
    public void UpdateMailContainer(MailContainer mailContainer)
    {
        // Update mail container in the database. Implementation not required for this exercise.
    }
}