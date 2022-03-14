using MailContainerTest.Types;

namespace MailContainerTest.Data;

/// <summary>
/// Factory for creating data store instances.
/// </summary>
public interface IMailContainerDataStoreFactory
{
    /// <summary>
    /// Creates an instance of a data store based on the supplied parameters.
    /// </summary>
    /// <param name="dataStoreType">The type of data store to create.</param>
    /// <returns>The data store instance.</returns>
    IMailContainerDataStore Create(MailContainerDataStoreType dataStoreType);
}