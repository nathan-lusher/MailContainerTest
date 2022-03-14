using MailContainerTest.Types;

namespace MailContainerTest.Data;

/// <summary>
/// Data store for container objects.
/// </summary>
public interface IMailContainerDataStore
{
    /// <summary>
    /// Retrieves a mail container from the data store.
    /// </summary>
    /// <param name="mailContainerNumber">The number of the mail container to retrieve.</param>
    /// <returns>The mail container.</returns>
    MailContainer GetMailContainer(string mailContainerNumber);

    /// <summary>
    /// Updates a container in the data store.
    /// </summary>
    /// <param name="mailContainer">The mail container to update.</param>
    void UpdateMailContainer(MailContainer mailContainer);
}